using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class UI_CharacterInfo : MonoBehaviour {

    //소환 상점에서 필요한 캐릭터 인포를 담아놓는곳
    public DBBasicCharacter basicCharacter;

    public Animator animator;
    public AnimationClip[] animationClip;
    private int animationCount = 0;

    private void Start()
    {
        Init();
    }


    public void Init()
    {

        animator = this.gameObject.GetComponent<Animator>();
        animationClip = animator.runtimeAnimatorController.animationClips;
        animationCount = animationClip.Length;

        //for (int i = 0; i < animationClip.Length; i++)
        //    Debug.Log(animationClip[i].name);

        //Debug.Log("애니메이션 갯수 : " + animationCount);
        //Debug.Log("현재 애니메이션 정보 : " + animator.GetCurrentAnimatorStateInfo(0).IsName(animationClip[0].name));

        StartCoroutine(ChagneAnimationLoop());
    }

    IEnumerator ChagneAnimationLoop()
    {
        yield return null;

        int random = 0;

        while (true)
        {
            //해당 애니메이션이 끝났는지 아닌지 확인 한다.
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationClip[random].name))//&& animator.GetCurrentAnimatorStateInfo(0).length >= animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                //Debug.Log("애니메이션 끝");
                random = Random.RandomRange(0, animationCount);
                animator.StopPlayback();

                animator.Play(animationClip[random].name, 0, 0f);

            }
            else
            {
                //Debug.Log("애니메이션 계속중...");
            }
            //해당 애니메이션의 길이 만큼 기다린다.
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }

        yield return null;


    }
}
