namespace HSM {
    public class PlayerRoot : State {
        public readonly Grounded Grounded;
        public readonly Airborne Airborne;
        public readonly Attacking Attacking;
        public readonly Hitted Hitted;
        readonly PlayerContext ctx;

        public PlayerRoot(StateMachine m, PlayerContext ctx) : base(m, null) {
            this.ctx = ctx;
            Grounded = new Grounded(m, this, ctx);
            Airborne = new Airborne(m, this, ctx);
            Attacking = new Attacking(m, this, ctx);
            Hitted = new Hitted(m, this, ctx);
        }
        
        protected override State GetInitialState() => Grounded;
        protected override State GetTransition() => null;
    }
}