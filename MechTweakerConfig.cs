using Newtonsoft.Json;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace MechTweaker
{
	[Label("Mech Tweaker Config")]
	public class MechTweakerConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("Statue Limits")]
		[DefaultValue(1f)]
		[Range(.25f, 5f)]
		[Increment(.25f)]
		[Label("Item Limit Multiplier")]
		[Tooltip("Adjusts the limit for each different statue spawned Item type.")]
		public float ItemLimitMultiplier { get; set; }

		[DefaultValue(1f)]
		[Range(.25f, 5f)]
		[Increment(.25f)]
		[Label("NPC Limit Multiplier")]
		[Tooltip("Adjusts the limit for each different statue spawned NPC type.")]
		public float NPCLimitMultiplier { get; set; }

		// TODO: spikyball projectile limit adjust

		[Header("Speed Multipliers")]
		[DefaultValue(1f)]
		[Range(.25f, 50f)]
		[Increment(.25f)]
		[Label("Mechanic Speed Multiplier")]
		[Tooltip("Adjusts Mechanical (Statue and Trap) cooldowns.")]
		public float MechSpeedMultiplier { get; set; }

		[DefaultValue(1), Range(1, 10)]
		[Label("Timer Speed Multiplier")]
		[Tooltip("Adjusts 1, 3, and 5 second timers. Mechanic Speed Multiplier needs to be adjusted as well if timers are triggering them.\nNon-Mech items like torches are not limited by the Mechanical cooldown.")]
		public int TimerSpeedMultiplier { get; set; }

		[Header("Resulting Times")]
		[JsonIgnore]
		[Range(0f, 1f)]
		[Label("1 Second Timer")]
		[Tooltip("Default time is 1 second")]
		public float OneSecondTimer => 60f / TimerSpeedMultiplier / 60;

		[JsonIgnore]
		[Range(0f, 3f)]
		[Label("3 Second Timer")]
		[Tooltip("Default time is 3 seconds")]
		public float ThreeSecondTimer => 180f / TimerSpeedMultiplier / 60;

		[JsonIgnore]
		[Range(0f, 5f)]
		[Label("5 Second Timer")]
		[Tooltip("Default time is 5 seconds")]
		public float FiveSecondTimer => 300f / TimerSpeedMultiplier / 60;

		[JsonIgnore]
		[Range(0f, 4f)]
		[Label("Dart/Flame Trap Cooldown")]
		[Tooltip("Default time is 3.33 seconds")]
		public float DartTrapTimer => 200f / MechSpeedMultiplier / 60;

		[JsonIgnore]
		[Range(0f, 4f)]
		[Label("Spiky Ball Trap Cooldown")]
		[Tooltip("Default time is 5 seconds\n(There are separate limits for the Spiky Ball itself, however, which are not implemented yet)")]
		public float SpikyBallTrapTimer => 300f / MechSpeedMultiplier / 60;

		[JsonIgnore]
		[Range(0f, 4f)]
		[Label("Spear Trap Cooldown")]
		[Tooltip("Default time is 1.5 seconds")]
		public float SpearTrapTimer => 90f / MechSpeedMultiplier / 60;

		[JsonIgnore]
		[Range(0f, 1f)]
		[Label("NPC Statue Cooldown")]
		[Tooltip("Default time is 0.5 seconds")]
		public float NPCStatueTimer => 30f / MechSpeedMultiplier / 60;

		[JsonIgnore]
		[Range(0f, 10f)]
		[Label("Item Statue Cooldown")]
		[Tooltip("Default time is 10 seconds")]
		public float ItemStatueTimer => 600f / MechSpeedMultiplier / 60;

		[JsonIgnore]
		[Range(0, 15)]
		[Label("Near Statue Limit")]
		public int NearStatueLimit => (int)(3 * NPCLimitMultiplier);

		[JsonIgnore]
		[Range(0, 12)]
		[Label("Far Statue Limit")]
		public int FarStatueLimit => (int)(6 * NPCLimitMultiplier);

		[JsonIgnore]
		[Range(0, 30)]
		[Label("Total Statue Limit")]
		public int TotalStatueLimit => (int)(10 * NPCLimitMultiplier);

		[JsonIgnore]
		[Range(0, 15)]
		[Label("Near Item Limit")]
		public int NearItemLimit => (int)(3 * ItemLimitMultiplier);

		[JsonIgnore]
		[Range(0, 12)]
		[Label("Far Item Limit")]
		public int FarItemLimit => (int)(6 * ItemLimitMultiplier);

		[JsonIgnore]
		[Range(0, 30)]
		[Label("Total Item Limit")]
		public int TotalItemLimit => (int)(10 * ItemLimitMultiplier);

		public override void OnLoaded() {
			MechTweaker.mechTweakerConfig = this;
		}
	}
}