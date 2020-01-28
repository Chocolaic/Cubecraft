using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class AbstractBlock : IBlock
{
    public static PaletteMapping Palette { get; set; } = new PaletteMapping();
    private ushort blockIdAndMeta;
    public int BlockID
    {
        get
        {
            if (Palette.IdHasMetadata)
            {
                return blockIdAndMeta >> 4;
            }
            return blockIdAndMeta;
        }
        set
        {
            if (Palette.IdHasMetadata)
            {
                if (value > (ushort.MaxValue >> 4) || value < 0)
                    throw new ArgumentOutOfRangeException("value", "Invalid block ID. Accepted range: 0-4095");
                blockIdAndMeta = (ushort)(value << 4 | BlockMeta);
            }
            else
            {
                if (value > ushort.MaxValue || value < 0)
                    throw new ArgumentOutOfRangeException("value", "Invalid block ID. Accepted range: 0-65535");
                blockIdAndMeta = (ushort)value;
            }
        }
    }
    public byte BlockMeta
    {
        get
        {
            if (Palette.IdHasMetadata)
            {
                return (byte)(blockIdAndMeta & 0x0F);
            }
            return 0;
        }
        set
        {
            if (Palette.IdHasMetadata)
            {
                blockIdAndMeta = (ushort)((blockIdAndMeta & ~0x0F) | (value & 0x0F));
            }
        }
    }
    public BlockMaterial Type
    {
        get
        {
            return Palette.FromId(BlockID);
        }
    }
    public AbstractBlock(ushort typeAndMeta)
    {
        this.blockIdAndMeta = typeAndMeta;
    }
}
