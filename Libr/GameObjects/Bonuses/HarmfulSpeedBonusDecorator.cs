namespace Libr
{
    public class HarmfulSpeedBonusDecorator(Tank player) : BonusDecorator(player)
    {
        private readonly float speedEffect = 0.15f;
        private readonly float speedNormal = 0.35f;

        public override void ActivateBonus()
        {
            if(_player.Speed >= speedNormal)
                _player.Speed -= speedEffect;
        }

        public override void DeactivateBonus()
        {
            if(_player.Speed < speedNormal)
                _player.Speed += speedEffect;
        }
    }
}
