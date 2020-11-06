﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;

public class TestInputLoadFont : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_InputField;
    [SerializeField] private Button m_Button;
    [SerializeField] private FontLoad m_Target;

    [SerializeField] private Button m_LoadSceneButton;
    [SerializeField] private string m_Scene;
    void Start()
    {
        m_Button.OnClickAsObservable().Subscribe(_ => m_Target.LoadAsset(m_InputField.text)).AddTo(this);
        m_LoadSceneButton.OnClickAsObservable().Subscribe(_ => SceneManager.LoadScene(m_Scene)).AddTo(this);
    }
}