namespace Outlander.Player
{
    public class OutlanderStateData
    {
        private PlayerOutlanderStateMachine context;
        private OutlanderAxeState axeState;
        private OutlanderBowState bowState;
        private OutlanderDieState dieState;
        private OutlanderLanceState lanceState;
        private OutlanderStunState stunState;
        private OutlanderSwordState swordState;
        private OutlanderNonWeaponState handState;

        public OutlanderStateData(PlayerOutlanderStateMachine outlanderStateMachine)
        {
            context = outlanderStateMachine;
            axeState = new OutlanderAxeState(context, this);
            bowState = new OutlanderBowState(context, this);
            dieState = new OutlanderDieState(context, this);
            lanceState = new OutlanderLanceState(context, this);
            stunState = new OutlanderStunState(context, this);
            swordState = new OutlanderSwordState(context, this);
            handState = new OutlanderNonWeaponState(context, this);
        }

        public OutlanderBaseState Axe() => axeState;
        public OutlanderBaseState Bow() => bowState;
        public OutlanderBaseState Die() => dieState;
        public OutlanderBaseState Lance() => lanceState;
        public OutlanderBaseState Stun() => stunState;
        public OutlanderBaseState Sword() => swordState;
        public OutlanderBaseState Hand() => handState;

        //public OutlanderBaseState Axe() => axeState.SetStateData(this);
        //public OutlanderBaseState Bow() => bowState.SetStateData(this);
        //public OutlanderBaseState Die() => dieState.SetStateData(this);
        //public OutlanderBaseState Lance() => lanceState.SetStateData(this);
        //public OutlanderBaseState Stun() => stunState.SetStateData(this);
        //public OutlanderBaseState Sword() => swordState.SetStateData(this);
        //public OutlanderBaseState Hand() => handState.SetStateData(this);
    }
}

