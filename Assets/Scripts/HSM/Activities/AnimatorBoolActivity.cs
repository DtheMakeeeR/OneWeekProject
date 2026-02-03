using HSM;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace HSM
{
    public class AnimatorBoolActivity : Activity
    {
        readonly Animator _anim;
        readonly string _name;
        readonly bool _startVal;
        readonly bool _endVal;
        public AnimatorBoolActivity(Animator anim, string name, bool startVal, bool endVal)
        {
            _anim = anim;
            _name = name;
            _startVal = startVal;
            _endVal = endVal;
        }

        public override async Task ActivateAsync(CancellationToken ct)
        {
            Debug.Log("ActivateAsync");
            if (this.Mode != ActivityMode.Inactive || _anim == null) return;
            this.Mode = ActivityMode.Activating;
            Debug.Log($"SET BOOL act {_name}");
            _anim.SetBool(_name, _startVal);
            this.Mode = ActivityMode.Active;
        }

        public override async Task DeactivateAsync(CancellationToken ct)
        {
            if (this.Mode != ActivityMode.Active || _anim == null) return;
            this.Mode = ActivityMode.Deactivating;
            _anim.SetBool(_name, _endVal);
            Debug.Log($"SET BOOL deact {_name}");
            this.Mode = ActivityMode.Inactive;
        }
    }
}
