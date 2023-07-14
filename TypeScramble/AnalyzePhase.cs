﻿using Confuser.Core;
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeScramble.Rewrite;

namespace TypeScramble {
    class AnalyzePhase : ProtectionPhase {

        public AnalyzePhase(TypeScrambleProtection parent) : base(parent) {

        }

        public override ProtectionTargets Targets => ProtectionTargets.Methods;

        public override string Name => "Typescramble analysis";

        protected override void Execute(ConfuserContext context, ProtectionParameters parameters) {

            var service = context.Registry.GetService<ITypeService>();

            foreach(IFactory f in service.Factories) {
                f.Reset();
            }

            foreach(var t in parameters.Targets.WithProgress(context.Logger).OfType<TypeDef>()) {
                if (t.HasGenericParameters) {
                    continue;
                }
                foreach(var f in t.Fields) {
                    service.AddAssociatedType(t, f.FieldType);
                }
            }


            foreach (var m in parameters.Targets.WithProgress(context.Logger).OfType<MethodDef>()) {

                if(!service.ShouldModify(m)) {
                    continue;
                }

                foreach (var v in m.Body.Variables) {
                    service.AddAssociatedType(m, v.Type);
                }

                if (m.ReturnType != m.Module.CorLibTypes.Void) {
                    service.AddAssociatedType(m, m.ReturnType);
                }

                foreach (var param in m.Parameters) {
                    if (param.Index == 0 && !m.IsStatic) {
                        continue;
                    }
                    service.AddAssociatedType(m, param.Type);
                }

                service.AnalizeMethod(m);

            }

        }
    }
}
