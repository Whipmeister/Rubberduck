using System.Collections;
using System.Collections.Generic;
using Rubberduck.VBEditor.SafeComWrappers.VB.Abstract;
using VBAIA = Microsoft.Vbe.Interop;

namespace Rubberduck.VBEditor.SafeComWrappers.VB.VBA
{
    public class Properties : SafeComWrapper<VBAIA.Properties>, IProperties
    {
        public Properties(VBAIA.Properties target) 
            : base(target)
        {
        }

        public int Count => IsWrappingNullReference ? 0 : Target.Count;

        public IVBE VBE => new VBE(IsWrappingNullReference ? null : Target.VBE);

        public IApplication Application => new Application(IsWrappingNullReference ? null : Target.Application);

        public object Parent => IsWrappingNullReference ? null : Target.Parent;

        public IProperty this[object index] => new Property(IsWrappingNullReference ? null : Target.Item(index));

        IEnumerator<IProperty> IEnumerable<IProperty>.GetEnumerator()
        {
            return IsWrappingNullReference
                ? new ComWrapperEnumerator<IProperty>(null, o => new Property(null))
                : new ComWrapperEnumerator<IProperty>(Target, o => new Property((VBAIA.Property) o));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return IsWrappingNullReference
                ? (IEnumerator) new List<IEnumerable>().GetEnumerator()
                : ((IEnumerable<IProperty>) this).GetEnumerator();
        }

        //public override void Release(bool final = false)
        //{
        //    if (!IsWrappingNullReference)
        //    {
        //        for (var i = 1; i <= Count; i++)
        //        {
        //            this[i].Release();
        //        }
        //        base.Release(final);
        //    }
        //}

        public override bool Equals(ISafeComWrapper<VBAIA.Properties> other)
        {
            return IsEqualIfNull(other) || (other != null && ReferenceEquals(other.Target, Target));
        }

        public bool Equals(IProperties other)
        {
            return Equals(other as SafeComWrapper<VBAIA.Properties>);
        }

        public override int GetHashCode()
        {
            return IsWrappingNullReference ? 0 : Target.GetHashCode();
        }
    }
}