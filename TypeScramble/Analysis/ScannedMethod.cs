using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScramble.Analysis {
    internal class ScannedMethod  : ScannedItem {

        public MethodDef TargetMethod { get; }

        public override MDToken MDToken => TargetMethod.MDToken;

        public ScannedMethod(MethodDef target) {
            TargetMethod = target;
        }


        public override void CreateGenerics() {
            Generics.Clear();
            GenericCallTypes.Clear();

            foreach (TypeSig t in AssociatedTypes) {
                if (!Generics.ContainsKey(t.ScopeType.MDToken.Raw)) {
                    Generics.Add(t.ScopeType.MDToken.Raw,
                        new GenericParamUser(
                            (ushort)(TargetMethod.DeclaringType.GenericParameters.Count + TargetMethod.GenericParameters.Count + Generics.Count()),
                            GenericParamAttributes.NoSpecialConstraint, GenericParamName)); //gen name
                    GenericCallTypes.Add(t);
                }
            }

        }

        public override void AddGenerticParam(GenericParam param) => TargetMethod.GenericParameters.Add(param);
    }
}
