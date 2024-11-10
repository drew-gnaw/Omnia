namespace Enemies.Charger
{
    public partial class Charger
    {
        private class WalkAnimation : IState
        {
            private readonly Charger charger;

            public WalkAnimation(Charger charger)
            {
                this.charger = charger;
            }

            public void OnEnter()
            {
                charger.animator.Play("ChargerWalk");
            }

            public void OnExit()
            {
            }

            public void Update()
            {
                charger.sprite.flipX = charger.rb.velocity.x > 0;
            }

            public void FixedUpdate()
            {
            }
        }
    }
}