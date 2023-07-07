using _Scripts.Characters.Adventurers;

namespace _Scripts.Characters.Animations
{
    public class BowmanAnimator : CharacterAnimator
    {
        #region Properties
        public Bowman MyBowman { get; private set; }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            if (!ViewIsMine())
                return;

            base.Awake();
            MyBowman = GetComponentInParent<Bowman>();
        }
        #endregion

        #region Methods
        public override void LaunchProjectile()
        {
            if (!ViewIsMine() || !_lastProjectile)
                return;

            _lastProjectile.transform.SetParent(null);
            _lastProjectile.OverrideProjectileDamages(Character.CharacterDatas.GetAttackDamages(projectileMainAttack));
            _lastProjectile.ThrowProjectile(Character.MainCamera.forward);
            _lastProjectile = null;
        }
        #endregion
    }
}
