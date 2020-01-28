using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PaletteMapping
{
    private static Dictionary<int, BlockMaterial> materials = new Dictionary<int, BlockMaterial>()
        {
            { 0, BlockMaterial.Air },
            { 1, BlockMaterial.Stone },
            { 2, BlockMaterial.GrassBlock },                      // Grass
            { 3, BlockMaterial.Dirt },
            { 4, BlockMaterial.Cobblestone },
            { 5, BlockMaterial.OakPlanks },                       // Wood:0
            { 6, BlockMaterial.OakSapling },                      // Sapling:0
            { 7, BlockMaterial.Bedrock },
            { 8, BlockMaterial.Water },                           // FlowingWater
            { 9, BlockMaterial.Water },                           // StationaryWater
            { 10, BlockMaterial.Lava },                           // FlowingLava
            { 11, BlockMaterial.Lava },                           // StationaryLava
            { 12, BlockMaterial.Sand },
            { 13, BlockMaterial.Gravel },
            { 14, BlockMaterial.GoldOre },
            { 15, BlockMaterial.IronOre },
            { 16, BlockMaterial.CoalOre },
            { 17, BlockMaterial.OakLog },                         // Log:0
            { 18, BlockMaterial.OakLeaves },                      // Leaves:0
            { 19, BlockMaterial.Sponge },
            { 20, BlockMaterial.Glass },
            { 21, BlockMaterial.LapisOre },
            { 22, BlockMaterial.LapisBlock },
            { 23, BlockMaterial.Dispenser },
            { 24, BlockMaterial.Sandstone },
            { 25, BlockMaterial.NoteBlock },
            { 26, BlockMaterial.RedBed },                         // Bed:0
            { 27, BlockMaterial.PoweredRail },
            { 28, BlockMaterial.DetectorRail },
            { 29, BlockMaterial.StickyPiston },                   // PistonStickyBase
            { 30, BlockMaterial.Cobweb },                         // Web
            { 31, BlockMaterial.Grass },                          // LongGrass
            { 32, BlockMaterial.DeadBush },
            { 33, BlockMaterial.Piston },                         // PistonBase
            { 34, BlockMaterial.PistonHead },                     // PistonExtension
            { 35, BlockMaterial.WhiteWool },                      // Wool:0
            { 36, BlockMaterial.MovingPiston },                   // PistonMovingPiece
            { 37, BlockMaterial.Dandelion },                      // YellowFlower
            { 38, BlockMaterial.Poppy },                          // RedRose
            { 39, BlockMaterial.BrownMushroom },
            { 40, BlockMaterial.RedMushroom },
            { 41, BlockMaterial.GoldBlock },
            { 42, BlockMaterial.IronBlock },
            { 43, BlockMaterial.StoneSlab },                      // DoubleStep
            { 44, BlockMaterial.StoneSlab },                      // Step
            { 45, BlockMaterial.Bricks },                         // Brick
            { 46, BlockMaterial.Tnt },
            { 47, BlockMaterial.Bookshelf },
            { 48, BlockMaterial.MossyCobblestone },
            { 49, BlockMaterial.Obsidian },
            { 50, BlockMaterial.Torch },
            { 51, BlockMaterial.Fire },
            { 52, BlockMaterial.Spawner },                        // MobSpawner
            { 53, BlockMaterial.OakStairs },                      // WoodStairs:0
            { 54, BlockMaterial.Chest },
            { 55, BlockMaterial.RedstoneWire },
            { 56, BlockMaterial.DiamondOre },
            { 57, BlockMaterial.DiamondBlock },
            { 58, BlockMaterial.CraftingTable },                  // Workbench
            { 59, BlockMaterial.Wheat },                          // Crops
            { 60, BlockMaterial.Farmland },                       // Soil
            { 61, BlockMaterial.Furnace },                        // Furnace
            { 62, BlockMaterial.Furnace },                        // BurningFurnace
            { 63, BlockMaterial.OakWallSign },                    // SignPost
            { 64, BlockMaterial.OakDoor },                        // WoodenDoor:0
            { 65, BlockMaterial.Ladder },
            { 66, BlockMaterial.Rail },                           // Rails
            { 67, BlockMaterial.CobblestoneStairs },
            { 68, BlockMaterial.OakWallSign },                    // WallSign
            { 69, BlockMaterial.Lever },
            { 70, BlockMaterial.StonePressurePlate },             // StonePlate
            { 71, BlockMaterial.IronDoor },                       // IronDoorBlock
            { 72, BlockMaterial.OakPressurePlate },               // WoodPlate:0
            { 73, BlockMaterial.RedstoneOre },                    // RedstoneOre
            { 74, BlockMaterial.RedstoneOre },                    // GlowingRedstoneOre
            { 75, BlockMaterial.RedstoneTorch },                  // RedstoneTorchOff
            { 76, BlockMaterial.RedstoneTorch },                  // RedstoneTorchOn 
            { 77, BlockMaterial.StoneButton },
            { 78, BlockMaterial.Snow },
            { 79, BlockMaterial.Ice },
            { 80, BlockMaterial.SnowBlock },
            { 81, BlockMaterial.Cactus },
            { 82, BlockMaterial.Clay },
            { 83, BlockMaterial.SugarCane },                      // SugarCaneBlock
            { 84, BlockMaterial.Jukebox },
            { 85, BlockMaterial.OakFence },                       // Fence:0
            { 86, BlockMaterial.Pumpkin },
            { 87, BlockMaterial.Netherrack },
            { 88, BlockMaterial.SoulSand },
            { 89, BlockMaterial.Glowstone },
            { 90, BlockMaterial.NetherPortal },                   // Portal
            { 91, BlockMaterial.JackOLantern },
            { 92, BlockMaterial.Cake },                           // CakeBlock
            { 93, BlockMaterial.Repeater },                       // DiodeBlockOff
            { 94, BlockMaterial.Repeater },                       // DiodeBlockOn
            { 95, BlockMaterial.WhiteStainedGlass },              // StainedGlass:0
            { 96, BlockMaterial.OakTrapdoor },                    // TrapDoor
            { 97, BlockMaterial.InfestedStone },                  // MonsterEggs:0
            { 98, BlockMaterial.StoneBricks },                    // SmoothBrick
            { 99, BlockMaterial.BrownMushroomBlock },             // HugeMushroom1
            { 100, BlockMaterial.BrownMushroomBlock },            // HugeMushroom2
            { 101, BlockMaterial.IronBars },                      // IronFence
            { 102, BlockMaterial.GlassPane },                     // ThinGlass
            { 103, BlockMaterial.Melon },                         // MelonBlock
            { 104, BlockMaterial.PumpkinStem },
            { 105, BlockMaterial.MelonStem },
            { 106, BlockMaterial.Vine },
            { 107, BlockMaterial.OakFenceGate },                  // FenceGate:0
            { 108, BlockMaterial.BrickStairs },
            { 109, BlockMaterial.StoneBrickStairs },              // SmoothStairs
            { 110, BlockMaterial.Mycelium },                      // Mycel
            { 111, BlockMaterial.LilyPad },                       // WaterLily
            { 112, BlockMaterial.NetherBricks},                   // NetherBrick
            { 113, BlockMaterial.NetherBrickFence },              // NetherFence
            { 114, BlockMaterial.NetherBrickStairs },
            { 115, BlockMaterial.NetherWart },                    // NetherWarts
            { 116, BlockMaterial.EnchantingTable },               // EnchantmentTable
            { 117, BlockMaterial.BrewingStand },
            { 118, BlockMaterial.Cauldron },
            { 119, BlockMaterial.EndPortal },                     // EnderPortal
            { 120, BlockMaterial.EndPortalFrame },                // EnderPortalFrame
            { 121, BlockMaterial.EndStone },                      // EnderStone
            { 122, BlockMaterial.DragonEgg },
            { 123, BlockMaterial.RedstoneLamp },                  // RedstoneLampOff
            { 124, BlockMaterial.RedstoneLamp },                  // RedstoneLampOn
            { 125, BlockMaterial.OakSlab },                       // WoodDoubleStep:0
            { 126, BlockMaterial.OakSlab },                       // WoodStep
            { 127, BlockMaterial.Cocoa },
            { 128, BlockMaterial.SandstoneStairs },
            { 129, BlockMaterial.EmeraldOre },
            { 130, BlockMaterial.EnderChest },
            { 131, BlockMaterial.TripwireHook },
            { 132, BlockMaterial.Tripwire },
            { 133, BlockMaterial.EmeraldBlock },
            { 134, BlockMaterial.SpruceStairs },                  // SpruceWoodStairs
            { 135, BlockMaterial.BirchStairs },                   // BirchWoodStairs
            { 136, BlockMaterial.JungleStairs },                  // JungleWoodStairs
            { 137, BlockMaterial.CommandBlock },                  // Command
            { 138, BlockMaterial.Beacon },
            { 139, BlockMaterial.CobblestoneWall },               // CobbleWall
            { 140, BlockMaterial.FlowerPot },
            { 141, BlockMaterial.Carrots },                       // Carrot
            { 142, BlockMaterial.Potatoes },                      // Potato
            { 143, BlockMaterial.OakButton },                     // WoodButton
            { 144, BlockMaterial.SkeletonSkull },                 // Skull:0
            { 145, BlockMaterial.Anvil },
            { 146, BlockMaterial.TrappedChest },
            { 147, BlockMaterial.LightWeightedPressurePlate },    // GoldPlate
            { 148, BlockMaterial.HeavyWeightedPressurePlate },    // IronPlate
            { 149, BlockMaterial.Comparator },                    // RedstoneComparatorOff
            { 150, BlockMaterial.Comparator },                    // RedstoneComparatorOn
            { 151, BlockMaterial.DaylightDetector },
            { 152, BlockMaterial.RedstoneBlock },
            { 153, BlockMaterial.QuartzBlock },                   // QuartzOre
            { 154, BlockMaterial.Hopper },
            { 155, BlockMaterial.QuartzBlock },
            { 156, BlockMaterial.QuartzStairs },
            { 157, BlockMaterial.ActivatorRail },
            { 158, BlockMaterial.Dropper },
            { 159, BlockMaterial.WhiteConcrete },                 // StainedClay:0
            { 160, BlockMaterial.WhiteStainedGlassPane },         // StainedGlassPane:0
            { 161, BlockMaterial.OakLeaves },                     // Leaves2:0
            { 162, BlockMaterial.OakLog },                        // Log2:0
            { 163, BlockMaterial.AcaciaStairs },
            { 164, BlockMaterial.DarkOakStairs },
            { 170, BlockMaterial.HayBlock },
            { 171, BlockMaterial.WhiteCarpet },                   // Carpet:0
            { 172, BlockMaterial.WhiteConcrete },                 // HardClay
            { 173, BlockMaterial.CoalBlock },
            { 174, BlockMaterial.PackedIce },
            { 175, BlockMaterial.TallGrass },                     // DoublePlant
        };
    protected Dictionary<int, BlockMaterial> GetDict()
    {
        return materials;
    }

    public bool IdHasMetadata
    {
        get
        {
            return true;
        }
    }
    public BlockMaterial FromId(int id)
    {
        Dictionary<int, BlockMaterial> materials = GetDict();
        if (materials.ContainsKey(id))
            return materials[id];
        return BlockMaterial.Air;
    }
}
