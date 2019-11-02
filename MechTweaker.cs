using MonoMod.Cil;
using System;
using Terraria.ModLoader;

using static Mono.Cecil.Cil.OpCodes;

namespace MechTweaker
{
	internal class MechTweaker : Mod
	{
		internal static MechTweakerConfig mechTweakerConfig;

		public override void Load() {
			IL.Terraria.NPC.MechSpawn += NPC_MechSpawn;
			IL.Terraria.Item.MechSpawn += Item_MechSpawn;
			IL.Terraria.Wiring.CheckMech += Wiring_CheckMech;
			IL.Terraria.Wiring.UpdateMech += Wiring_UpdateMech;
		}

		public override void Unload() {
			mechTweakerConfig = null;
		}

		private void Wiring_UpdateMech(ILContext il) {
			// The math here is funky, least common denominator.

			var c = new ILCursor(il);

			if (!c.TryGotoNext(i => i.MatchLdcI4(300)))
				return; // Patch unable to be applied
			if (!c.TryGotoNext(i => i.MatchLdsfld("Terraria.Wiring", "_mechTime")))
				return;
			if (!c.TryGotoNext(i => i.MatchConvR8()))
				return;

			c.Index++;
			c.Index++;
			c.EmitDelegate<Func<int, int>>((timerTime) => {
				return (int)(timerTime / mechTweakerConfig.TimerSpeedMultiplier);
			});
		}

		private void Wiring_CheckMech(ILContext il) {
			var c = new ILCursor(il);
			c.Emit(Ldarg_2);
			c.EmitDelegate<Func<int, int>>((time) => {
				return (int)(time / mechTweakerConfig.MechSpeedMultiplier); // if 0, infinity.
			});
			c.Emit(Starg_S, (byte)2);
		}

		private void Item_MechSpawn(ILContext il) {
			var c = new ILCursor(il);

			if (!c.TryGotoNext(i => i.MatchLdcI4(3)))
				return; // Patch unable to be applied

			c.Index++;
			c.EmitDelegate<Func<int, int>>((nearLimit) => {
				return (int)(nearLimit * mechTweakerConfig.ItemLimitMultiplier);
			});

			c.GotoNext(i => i.MatchLdcI4(6));
			c.Index++;
			c.EmitDelegate<Func<int, int>>((farLimit) => {
				return (int)(farLimit * mechTweakerConfig.ItemLimitMultiplier);
			});

			c.GotoNext(i => i.MatchLdcI4(10));
			c.Index++;
			c.EmitDelegate<Func<int, int>>((totalLimit) => {
				return (int)(totalLimit * mechTweakerConfig.ItemLimitMultiplier);
			});
		}

		private void NPC_MechSpawn(MonoMod.Cil.ILContext il) {
			var c = new ILCursor(il);

			if (!c.TryGotoNext(i => i.MatchLdcI4(3)))
				return; // Patch unable to be applied

			c.Index++;
			c.EmitDelegate<Func<int, int>>((nearLimit) => {
				return (int)(nearLimit * mechTweakerConfig.NPCLimitMultiplier);
			});

			c.GotoNext(i => i.MatchLdcI4(6));
			c.Index++;
			c.EmitDelegate<Func<int, int>>((farLimit) => {
				return (int)(farLimit * mechTweakerConfig.NPCLimitMultiplier);
			});

			c.GotoNext(i => i.MatchLdcI4(10));
			c.Index++;
			c.EmitDelegate<Func<int, int>>((totalLimit) => {
				return (int)(totalLimit * mechTweakerConfig.NPCLimitMultiplier);
			});
		}

		// misc work.
		//var l = (ILLabel)c.Next.Operand;
		//c.GotoLabel(l);

		//c.MoveAfterLabels();
		//c.Emit(Ldloc_1);
		//c.EmitDelegate<Func<int, int>>((timerTime) => {
		//	return (int)(timerTime / mechTweakerConfig.TimerSpeedMultiplier);
		//});
		//c.Emit(Stloc_1);

		//c.Index++;
		//c.Index++;
		//c.Emit(Ldloc_1);
		//c.EmitDelegate<Func<int, int>>((nearLimit) => {
		//	return (int)(nearLimit / mechTweakerConfig.TimerSpeedMultiplier);
		//});
		//c.Emit(Stloc_1);

		//var l = (ILLabel)c.Next.Next.Operand;
		//c.GotoLabel(l);

		//if (!c.TryGotoNext(i => i.MatchLdcI4(180)))
		//	return; // Patch unable to be applied

		//ILLabel l = null;
		//if (!c.TryGotoNext(i => i.MatchBr(out l)))
		//	return; // Patch unable to be applied

		////var l = (ILLabel)c.Prev.Operand;
		//c.GotoLabel(l);
	}
}