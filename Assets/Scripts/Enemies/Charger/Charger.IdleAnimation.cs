namespace Enemies.Charger
{
    public partial class Charger
    {
        private class IdleAnimation : IState
        {
            private readonly Charger charger;

            public IdleAnimation(Charger charger)
            {
                this.charger = charger;
            }

            public void OnEnter()
            {
                charger.animator.Play("ChargerIdle");
            }

            public void OnExit()
            {
            }

            public void Update()
            {
            }

            public void FixedUpdate()
            {
            }
        }
    }
}