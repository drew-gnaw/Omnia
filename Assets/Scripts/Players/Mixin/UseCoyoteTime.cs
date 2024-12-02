using System;
using System.Linq;
using UnityEngine;

namespace Players.Mixin {
    public class UseCoyoteTime : MonoBehaviour {
        [SerializeField] internal Player self;
        [SerializeField] internal float delay;

        private bool grounded;
        private Vector2 slide;
        private float gt;
        private float st;

        public void Update() {
            var touching = slide.x != 0;

            gt = grounded ? delay : Math.Max(0, gt - Time.deltaTime);
            st = touching ? delay : Math.Max(0, st - Time.deltaTime);
            self.grounded = gt > 0;
            self.slide = st > 0 ? touching ? slide : self.slide : Vector2.zero;
        }

        public void FixedUpdate() {
            var checks = self.checks.Select(it => it.IsTouchingLayers(self.ground)).ToArray();
            var l = checks[2] && self.moving.x < 0;
            var r = checks[0] && self.moving.x > 0;

            grounded = checks[1];
            slide = l ? Vector2.left : r ? Vector2.right : Vector2.zero;
        }
    }
}
