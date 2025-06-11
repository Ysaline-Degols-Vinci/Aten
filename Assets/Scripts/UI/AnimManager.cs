using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Cache tout au départ
        spriteRenderer.enabled = false;
        animator.enabled = false;
    }

    public void JouerAnimation()
    {

        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            Debug.Log("Animation trouvée : " + clip.name);
        }
        // Active le visuel et l'animator
        spriteRenderer.enabled = true;
        animator.enabled = true;
        animator.Rebind();
        animator.Update(0f);
        animator.Play("lucioleAnim"); // Nom exact de l'animation dans l'Animator
    }
}
