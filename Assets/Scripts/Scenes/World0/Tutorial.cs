using System;
using System.Collections;
using System.Collections.Generic;
using Enemies.Dummy;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

namespace Scenes {
    public class Tutorial : MonoBehaviour {
        [SerializeField] private Dinky dinky;
        [SerializeField] private ColliderEventBroadcaster dummyAirCheck;
        [SerializeField] private Transform dinkyAppearTransform;

        [SerializeField] private DialogueWrapper beginDialogue;
        [SerializeField] private DialogueWrapper platformDummyDialogue;
        [SerializeField] private DialogueWrapper pullToDummyDialogue;
        [SerializeField] private DialogueWrapper dinkyScaredDialogue;
        [SerializeField] private DialogueWrapper dinkyGoneDialogue;

        [SerializeField] private Dummy dummy1;
        [SerializeField] private Dummy dummy2;
        [SerializeField] private Dummy dummy3;
        [SerializeField] private Dummy dummy4;
        [SerializeField] private Dummy dummy5;

        private int dummiesHit = 0;

        private void Start() {
            List<Dummy> dummies = new() { dummy1, dummy2, dummy3, dummy4, dummy5 };
            foreach (var dummy in dummies) {
                if (dummy == null) {
                    Debug.LogWarning("A dummy is null, please check that they are non null");
                    return;
                }
                dummy.canBeHurt = false;
            }

            StartCoroutine(BeginSequence());

            if (dinky == null) Debug.LogError("Dinky is not assigned!");
            if (dummyAirCheck == null) Debug.LogError("Dummy Air Check is not assigned!");
            if (dinkyAppearTransform == null) Debug.LogError("Dinky Appear Transform is not assigned!");
            if (beginDialogue == null) Debug.LogError("Begin Dialogue is not assigned!");
            if (AudioManager.Instance == null) Debug.LogError("AudioManager Instance is null!");
            if (DialogueManager.Instance == null) Debug.LogError("DialogueManager Instance is null!");
            if (HighlightManager.Instance == null) Debug.LogError("HighlightManager Instance is null!");

        }

        private IEnumerator BeginSequence() {
            AudioManager.Instance.SwitchBGM(AudioTracks.SunkBeneath);

            dinky.Appear(dinkyAppearTransform);
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(beginDialogue.Dialogue));

            Rigidbody2D rb = dinky.AddComponent<Rigidbody2D>();
            float horizontalForce = UnityEngine.Random.Range(1.5f, 3f);
            float spinForce = UnityEngine.Random.Range(250f, 400f) * (UnityEngine.Random.value > 0.5f ? 1 : -1);
            rb.gravityScale = 1f;
            rb.velocity = new Vector2(horizontalForce, 15);
            rb.angularVelocity = spinForce;


            RegisterDummyHurt(dummy1);
            RegisterDummyHurt(dummy2);
            yield return new WaitUntil(() => dummiesHit == 2);

            StartCoroutine(PlatformDummySequence());
        }

        private void RegisterDummyHurt(Dummy dummy) {
            dummy.OnHurt += HandleDummyHurt;
            dummy.canBeHurt = true;
            HighlightManager.Instance.HighlightGameObject(dummy.gameObject);
        }

        private void HandleDummyHurt(Dummy dummy) {
            dummy.OnHurt -= HandleDummyHurt;
            HighlightManager.Instance.UnhighlightGameObject(dummy.gameObject);
            dummiesHit++;
        }

        private IEnumerator PlatformDummySequence() {
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(platformDummyDialogue.Dialogue));

            RegisterDummyHurt(dummy4);
            RegisterDummyHurt(dummy5);
            yield return new WaitUntil(() => dummiesHit == 4);

            RegisterDummyHurt(dummy3);
            yield return new WaitUntil(() => dummiesHit == 5);

            StartCoroutine(PullToDummySequence());
        }

        private IEnumerator PullToDummySequence() {
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(pullToDummyDialogue.Dialogue));
            dummyAirCheck.OnEnter += HandleHitAirDummy;
        }

        private void HandleHitAirDummy() {
            dummyAirCheck.OnEnter -= HandleHitAirDummy;
            StartCoroutine(BeginDummyFallSequence());
        }

        private IEnumerator BeginDummyFallSequence() {
            Rigidbody2D rb = dummy3.GetComponent<Rigidbody2D>();
            if (rb == null) {
                rb = dummy3.AddComponent<Rigidbody2D>();
            }

            rb.gravityScale = 1f;
            // always towards the right
            float horizontalForce = UnityEngine.Random.Range(1.5f, 3f);
            rb.velocity = new Vector2(horizontalForce, 0);

            float spinForce = UnityEngine.Random.Range(250f, 400f) * (UnityEngine.Random.value > 0.5f ? 1 : -1);
            rb.angularVelocity = spinForce;

            // dummy hits ground
            yield return new WaitUntil(() => rb.position.y < -4.5f);

            ScreenShakeManager.Instance.Shake(10f);

            yield return new WaitForSeconds(0.7f);

            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(dinkyScaredDialogue.Dialogue));

            yield return new WaitForSeconds(0.3f);
            dinky.Disappear();
            yield return new WaitForSeconds(1f);

            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(dinkyGoneDialogue.Dialogue));
            yield return new WaitForSeconds(0.5f);
            LevelManager.Instance.NextLevel();
        }


    }
}
