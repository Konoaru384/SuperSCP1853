using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;

namespace SuperSCP1853
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "SuperSCP1853";
        public override string Author => "Konoara";

        private const ItemType AntiSCP207 = (ItemType)46;
        private const ItemType Scp1853 = (ItemType)51;

        private readonly HashSet<Player> consumedAnti207 = new HashSet<Player>();
        private readonly HashSet<Player> consumed1853 = new HashSet<Player>();
        private readonly HashSet<Player> immunePlayers = new HashSet<Player>();

        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            consumedAnti207.Clear();
            consumed1853.Clear();
            immunePlayers.Clear();
        }

        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (!Config.IsEnabled)
                return;

            if (ev.Item.Type == AntiSCP207)
                consumedAnti207.Add(ev.Player);

            if (ev.Item.Type == Scp1853)
                consumed1853.Add(ev.Player);

            TryGrantImmunity(ev.Player);
        }

        private void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (!Config.IsEnabled)
                return;

            if (ev.Pickup.Type == AntiSCP207)
                consumedAnti207.Add(ev.Player);

            if (ev.Pickup.Type == Scp1853)
                consumed1853.Add(ev.Player);

            TryGrantImmunity(ev.Player);
        }

        private void TryGrantImmunity(Player player)
        {
            if (immunePlayers.Contains(player))
                return;

            if (consumedAnti207.Contains(player) && consumed1853.Contains(player))
            {
                immunePlayers.Add(player);
                player.DisableEffect(EffectType.Poisoned);
            }
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (!Config.IsEnabled)
                return;

            if (immunePlayers.Contains(ev.Player))
                ev.IsAllowed = false;
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            consumedAnti207.Clear();
            consumed1853.Clear();
            immunePlayers.Clear();
        }
    }
}
