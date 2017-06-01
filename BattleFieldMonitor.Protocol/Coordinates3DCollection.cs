using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    public class Coordinates3DCollection : BlittableCollection<Coordinates3D>
    {
        #region Properties
        protected override int SizeOfItem
        {
            get { return SizeOf.Coordinates3D; }
        }
        #endregion

        #region Methods
        protected override unsafe void ReadItems(Stream stream, Coordinates3D[] items, int start, int count)
        {
            fixed (Coordinates3D* p = &items[start])
            {
                stream.ReadFully((byte*)p, count * sizeof(Coordinates3D));

                for (var i = 0; i < count; ++i)
                {
                    p[i].SwapBytes();
                }
            }
        }

        protected override unsafe void WriteItems(Stream stream, Coordinates3D[] items, int start, int count)
        {
            fixed (Coordinates3D* p = &items[start])
            {
                for (var i = 0; i < count; ++i)
                {
                    p[i].SwapBytes();
                }

                stream.Write((byte*)p, count * sizeof(Coordinates3D));
            }
        }
        #endregion
    }
}