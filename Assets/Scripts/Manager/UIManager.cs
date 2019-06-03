﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour {
    [SerializeField]
    Text t_score;
    [SerializeField]
    Text t_skill;
    // Use this for initialization
    [SerializeField]
    RectTransform r_gameOver;
    [SerializeField]
    Image bg;
    [SerializeField]
    Button b_reStart;
    [SerializeField]
    Button b_back;
    [SerializeField]
    Slider s_capacity;

    [SerializeField]
    Image tbc;

    [SerializeField]
    Image fog;


    [SerializeField]
    RectTransform firstMeetUI;

    [SerializeField]
    Text t_meetFishId;

    [SerializeField]
    Button b_meetFishOk;

    [SerializeField]
    Image meetFishImg;
    private void Awake()
    {
        GameManager.UpdateUIHandler += UpdateUI;
        Obstacle.GameOverHandler += ShowGameOverUI;
        Fish.FirstMeetHandler += ShowFirstMeetUI;

        b_meetFishOk.onClick.AddListener(HideMeetUI); 
        fog.color = new Color(1, 1, 1, 0);
        fog.DOFade(1.0f, 0.0f);
    }

    private void ShowFirstMeetUI(int id) {
        Time.timeScale = 0;
        Tweener move = firstMeetUI.DOLocalMove(Vector3.zero, 1.0f);
        move.SetEase(Ease.InQuad);
        move.SetUpdate(true);
        t_meetFishId.text = SpawnManager.GetFishList().fish[id].name;
    }

    void HideMeetUI() {
        Time.timeScale = 1;
        Tweener move = firstMeetUI.DOLocalMove(Vector3.up*1600f, 1.0f);
        move.SetEase(Ease.InQuad);
        move.SetUpdate(true);
    }

    private void Start()
    {
        b_reStart.onClick.AddListener(delegate () { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); Time.timeScale = 1; });
        b_back.onClick.AddListener(delegate () { SceneManager.LoadScene(0); Time.timeScale = 1; });
        b_reStart.interactable = false;
        b_back.interactable = false;
        
        fog.DOFade(0.0f, 3.0f);
        
    }

    private void OnDestroy()
    {
        GameManager.UpdateUIHandler -= UpdateUI;
        Obstacle.GameOverHandler -= ShowGameOverUI;
        Fish.FirstMeetHandler -= ShowFirstMeetUI;
    }

    // Update is called once per frame
    void Update () {
		
	}

    void UpdateUI(int score, int skillTimes, float capacity) {
        t_score.text = "score:" + score.ToString();
        t_skill.text = "skill:" + skillTimes.ToString();
        s_capacity.DOValue(capacity > 1 ? 1 : capacity, 0.5f).SetUpdate(true); ;
    }

    void ShowGameOverUI(int i) {
        StartCoroutine(IEShowGameOverUI());
    }

    IEnumerator IEShowGameOverUI() {
        yield return new WaitForSeconds(1f);
        //Time.timeScale = 0;
        bg.DOFade(0.5f, 1f).SetUpdate(true);
        Tweener move = r_gameOver.DOLocalMove(Vector3.zero, 1.0f);
        move.SetEase(Ease.InQuad);
        move.SetUpdate(true);
        move.onComplete = delegate (){
            r_gameOver.DOShakeRotation(3.0f, 5.0f).SetUpdate(true);
            b_reStart.interactable = true;
            b_back.interactable = true;        
        };

        Tweener movetbc = tbc.rectTransform.DOAnchorPosX(0, 1.0f);
        movetbc.SetEase(Ease.InQuint);
        movetbc.SetUpdate(true);
    }

}
