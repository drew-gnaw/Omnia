using UnityEngine;
using System.Collections;
using Omnia.State;
using Omnia.Utils;

namespace Omnia.Player {
    public class PlayerController : MonoBehaviour {
        [Header("References")] public LayerMask groundLayer;
        public BoxCollider2D groundCheck;
        public BoxCollider2D leftWallCheck;
        public BoxCollider2D rightWallCheck;
        public Animator animator;
        public SpriteRenderer spriteRenderer;

        [Header("General Settings")] [SerializeField]
        float maxFallingSpeed = 10f;

        [SerializeField] float moveSpeed = 5f;

        [Header("Jump Settings")] [SerializeField]
        private float jumpForce = 7f;

        [SerializeField] float jumpDuration = 0.2f;
        [SerializeField] float jumpCooldown = 0f;
        [SerializeField] float gravityMultiplier = 3f;
        [SerializeField] float maxSlidingSpeed = 4f;
        [SerializeField] public float wallJumpPower = 2f;
        [SerializeField] public float wallJumpLockoutTime = 1f;
        [SerializeField] public float wallJumpCoyoteTime = 0.3f;

        public float attackCooldownDuration = 0.5f;

        private PlayerBase playerBase;
        public WeaponClass currentWeapon;
        public WeaponClass offhandWeapon;

        private Rigidbody2D rb;

        private bool isGrounded = false;
        public bool facing; // false = left
        public int lastWallDirection; // 1 = right, -1 = left, 0 = no wall

        public bool IsSlidingLeft { get; private set; } = false;
        public bool IsSlidingRight { get; private set; } = false;

        private StateMachine stateMachine;
        private IdleState idleState;
        private WalkState walkState;
        private JumpState jumpState;
        private SlideState slideState;
        private WallJumpState wallJumpState;
        private FallState fallState;

        // Timers
        private CountdownTimer jumpTimer;
        private CountdownTimer jumpCooldownTimer;
        private CountdownTimer wallJumpTimer;
        private CountdownTimer wallJumpLockoutTimer;
        public CountdownTimer wallJumpCoyoteTimer;
        private CountdownTimer attackTimer;

        private float yVelocity;
        private float inputXVelocity;
        private float externalXVelocity;

        private IEnumerator currentWallJumpCoroutine;

        void Start() {
            rb = GetComponent<Rigidbody2D>();
            SetupStateMachine();
            SetupTimers();
        }

        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

        void SetupStateMachine() {
            stateMachine = new StateMachine();

            idleState = new IdleState(this, animator);
            walkState = new WalkState(this, animator);
            jumpState = new JumpState(this, animator);
            slideState = new SlideState(this, animator);
            wallJumpState = new WallJumpState(this, animator);
            fallState = new FallState(this, animator);

            At(idleState, walkState, new FuncPredicate(() => Mathf.Abs(inputXVelocity) > 0.1f));
            At(idleState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));

            At(walkState, idleState, new FuncPredicate(ReturnToIdleState));
            At(fallState, idleState, new FuncPredicate(ReturnToIdleState));
            At(slideState, idleState, new FuncPredicate(() => isGrounded && Mathf.Abs(inputXVelocity) < 0.1f));

            At(walkState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
            Any(walkState, new FuncPredicate(ReturnTowalkState));

            At(jumpState, fallState, new FuncPredicate(() => rb.velocity.y < -0.1f));
            At(wallJumpState, fallState, new FuncPredicate(() => rb.velocity.y < -0.1f));
            At(walkState, fallState, new FuncPredicate(() => rb.velocity.y < -0.1f));

            At(fallState, slideState, new FuncPredicate(() => IsSlidingLeft || IsSlidingRight));
            At(slideState, fallState, new FuncPredicate(() => !IsSlidingLeft && !IsSlidingRight));


            At(slideState, wallJumpState, new FuncPredicate(() => wallJumpTimer.IsRunning));
            At(fallState, wallJumpState, new FuncPredicate(() => wallJumpTimer.IsRunning && wallJumpCoyoteTimer.IsRunning));
            At(jumpState, wallJumpState, new FuncPredicate(() => wallJumpTimer.IsRunning));

            stateMachine.SetState(idleState);
        }

        bool ReturnToIdleState() {
            return isGrounded && Mathf.Abs(inputXVelocity) < 0.1f;
        }

        bool ReturnTowalkState() {
            return isGrounded && !jumpTimer.IsRunning && !wallJumpTimer.IsRunning && Mathf.Abs(inputXVelocity) >= 0.1;
        }

        void SetupTimers() {
            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);
            wallJumpTimer = new CountdownTimer(jumpDuration);
            wallJumpLockoutTimer = new CountdownTimer(wallJumpLockoutTime);
            wallJumpCoyoteTimer = new CountdownTimer(wallJumpCoyoteTime);

            jumpTimer.OnTimerStart += () => yVelocity = jumpForce;
            wallJumpTimer.OnTimerStart += () => yVelocity = jumpForce;
            wallJumpTimer.OnTimerStart += () => wallJumpLockoutTimer.Start();
            wallJumpTimer.OnTimerStop += () => wallJumpCoyoteTimer.Stop();
            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();
            wallJumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();

            attackTimer = new CountdownTimer(attackCooldownDuration);
        }

        void Update() {
            stateMachine.Update();
            HandleGroundCheck();
            HandleWallCheck();
            HandleInput();
            HandleTimers();
        }

        void HandleTimers() {
            jumpTimer.Tick(Time.deltaTime);
            attackTimer.Tick(Time.deltaTime);
            wallJumpTimer.Tick(Time.deltaTime);
            wallJumpLockoutTimer.Tick(Time.deltaTime);
            wallJumpCoyoteTimer.Tick(Time.deltaTime);
        }

        void FixedUpdate() {
            stateMachine.FixedUpdate();
        }

        void HandleInput() {
            float moveInput = Input.GetAxis("Horizontal");
            HandleMovement(moveInput);

            if (Input.GetButtonDown("Jump")) {
                OnJump(true);
            }
            else if (Input.GetButtonUp("Jump")) {
                OnJump(false);
            }

            if (Input.GetButtonDown("Fire1") && !attackTimer.IsRunning) {
                HandleAttack();
            }
        }

        void OnJump(bool performed) {
            if (!isGrounded && (IsSlidingLeft || IsSlidingRight || wallJumpCoyoteTimer.IsRunning)) {
                if (performed && !wallJumpTimer.IsRunning) {
                    wallJumpTimer.Start();
                }
                else if (!performed && wallJumpTimer.IsRunning) {
                    wallJumpTimer.Stop();
                }
            }
            else {
                if (performed && !jumpTimer.IsRunning && isGrounded) {
                    jumpTimer.Start();
                }
                else if (!performed && jumpTimer.IsRunning) {
                    jumpTimer.Stop();
                }
            }
        }

        public void HandleJump() {
            if (!jumpTimer.IsRunning) {
                yVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }

            rb.velocity = new Vector2(rb.velocity.x, yVelocity);
        }

        public void HandleWallJump(int direction) {
            if (wallJumpTimer.IsRunning) {
                externalXVelocity = direction * wallJumpPower;
            }

            if (!wallJumpTimer.IsRunning) {
                yVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }

            rb.velocity = new Vector2(inputXVelocity + externalXVelocity, yVelocity);
        }

        public void HandleFall() {
            yVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            if (yVelocity < -maxFallingSpeed) {
                yVelocity = -maxFallingSpeed;
            }

            rb.velocity = new Vector2(rb.velocity.x, yVelocity);
        }

        public void HandleAttack() {
            currentWeapon.Attack();
        }

        public void HandleSlide() {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxSlidingSpeed));
        }

        void HandleGroundCheck() {
            isGrounded = groundCheck.IsTouchingLayers(groundLayer);
        }

        void HandleWallCheck() {
            bool wasSlidingLeft = IsSlidingLeft;
            bool wasSlidingRight = IsSlidingRight;

            bool isLeftKeyPressed = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            bool isRightKeyPressed = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

            IsSlidingLeft = leftWallCheck.IsTouchingLayers(groundLayer) && isLeftKeyPressed;
            IsSlidingRight = rightWallCheck.IsTouchingLayers(groundLayer) && isRightKeyPressed;

            if (IsSlidingLeft) {
                lastWallDirection = 1;
            }
            else if (IsSlidingRight) {
                lastWallDirection = -1;
            }

            // Start coyote timer if player just left the wall
            if ((wasSlidingLeft && !IsSlidingLeft) || (wasSlidingRight && !IsSlidingRight)) {
                wallJumpCoyoteTimer.Start();
            }
        }


        public void HandleMovement(float moveInput) {
            if (moveInput != 0) {
                facing = moveInput > 0;
            }

            spriteRenderer.flipX = !facing;

            if (wallJumpLockoutTimer.IsRunning && Mathf.Approximately(-lastWallDirection, Mathf.Sign(inputXVelocity))) {
                inputXVelocity = wallJumpLockoutTimer.Progress * moveInput * moveSpeed;
            }
            else {
                inputXVelocity = moveInput * moveSpeed;
            }

            rb.velocity = new Vector2(inputXVelocity + externalXVelocity, rb.velocity.y);
        }

        public void ZeroYVelocity() {
            yVelocity = 0f;
            rb.velocity = new Vector2(rb.velocity.x, yVelocity);
        }

        public IEnumerator WallJumpLockoutCoroutine(float direction) {
            if (currentWallJumpCoroutine != null) {
                StopCoroutine(currentWallJumpCoroutine);
            }

            currentWallJumpCoroutine = WallJumpLockoutCoroutineInternal(direction);
            yield return StartCoroutine(currentWallJumpCoroutine);
        }

        private IEnumerator WallJumpLockoutCoroutineInternal(float direction) {
            float duration = wallJumpLockoutTime;
            float elapsedTime = 0f;

            while (elapsedTime < duration) {
                float progress = elapsedTime / duration;
                externalXVelocity = (1 - progress) * direction * wallJumpPower;
                elapsedTime += Time.deltaTime;
                yield return null; // Waits for the next frame
            }

            externalXVelocity = 0f;
            currentWallJumpCoroutine = null; // Clear the reference when done
        }
    }
}