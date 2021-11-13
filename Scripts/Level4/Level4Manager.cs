using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Level4Manager : MonoBehaviour
{

    public static Level4Manager instance;

    public int setNumber;
    public int currentShapes;
    public int setOfUpperImages;
    public Level0MascotManager mascot;
    Level1Manager level1;
    [SerializeField] private Sprite[] triangleSprites;
    [SerializeField] private Sprite[] squareSprites;
    [SerializeField] private Sprite[] rectangleSprites;
    [SerializeField] private Sprite[] circleSprites;
    public Image[] shapes;
    [SerializeField] private Image[] boxesAndLines;
    [SerializeField] private RectTransform hand;
    public GameObject NonInteractableBoxParent;
    public GameObject InteractaleObjectsParent;
    [SerializeField] private GameObject InteractaleShapesParent;
    [SerializeField] private Image handImage;
    public GameObject confetti;
    public GameObject levelCompletedIcons;
    [SerializeField] private Button audioReplay;
    [SerializeField] private Image progressImage;
    [SerializeField] private Sprite[] progressSprites;
    public TextMeshProUGUI rewardsText;
    [SerializeField] private GameObject homeButtonIcons;
    [SerializeField] private GameObject skipButtonIcons;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button[] boxes;
    [SerializeField] private Button hintButton;
    [SerializeField] private GameObject presentationSkipButtonIcons;
    [SerializeField] private GameObject presentationEndIcons;

    private WaitForSeconds oneSec = new WaitForSeconds(1f);
    private Vector2 initMascotPos1;
    private bool mascotOnScreen;
    private int correctAnswerStreak;
    private bool firstAttempt;
    private int progress;
    private int correctShapeHolderIndex;
    private bool audioMute = false;
    private bool canSelectBox = false;
    private bool tutorial;
    private Coroutine voiceInstructionRepeatCoroutine;
    private Coroutine presentationCoroutine;
    private Coroutine nextShapeCoroutine;
    private bool menuButtonsOnScreen = false;
    bool onoff = false;
    public Image speechbubble;
    public Image speechDots;
    public GameObject handGameObject;
    public Image[] redBoxesImages;
    public Image[] greenBoxesImages;
    public Image audioReplayImage;
    public List<UpperImages> upperImages = new List<UpperImages>();
    public int shapesCount;
    public Image[] barImages;
    public Canvas canvas2;
    public Canvas canvas;
    public Sprite buttonImage;
    public Sprite newButtonImage;
    public Button musicButton;
    public Button backButton;
    public GameObject backButtonIcons;
    Coroutine co;

    private void Awake()
    {
        instance = this;
        //HandManagerLV1.selectedShape += CheckAnswer;
        presentationEndIcons.gameObject.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        presentationEndIcons.gameObject.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(PlayGame);
    }

    public bool MenuButtonsOnScreen
    {
        get
        {
            return menuButtonsOnScreen;
        }
        set
        {
            menuButtonsOnScreen = value;
            homeButton.interactable = !value;
            hintButton.interactable = !value;
            if (!Tutorial && !value)
            {
                audioReplay.interactable = !value;
            }
            else
            {
                audioReplay.interactable = !value;
            }
            skipButton.interactable = !value;

        }
    }

    public bool Tutorial
    {
        get
        {
            return tutorial;
        }
        set
        {
            tutorial = value;
            hintButton.interactable = !value;

        }
    }

    public bool AudioMute
    {
        get
        {
            return audioMute;
        }
        set
        {
            audioMute = value;
            if (audioMute == true)
            {
                ChangeButtonImage();
                AudioManager.instance.Stop("BackgroundMusic");
            }
            else
            {
                musicButton.image.sprite = buttonImage;
                AudioManager.instance.Play("BackgroundMusic");
            }
        }
    }

    public void ChangeButtonImage()
	{
        musicButton.image.sprite = newButtonImage;
	}

    public bool CanSelectBox
    {
        get
        {
            return canSelectBox;
        }
        set
        {
            if (!Tutorial && value)
            {
                canSelectBox = value;
                audioReplay.interactable = value;
                for (int i = 0; i < boxes.Length; i++)
                {
                    boxes[i].interactable = value;
                }
            }
            else
            {
                canSelectBox = value;
                audioReplay.interactable = value;
                for (int i = 0; i < boxes.Length; i++)
                {
                    boxes[i].interactable = value;
                }
            }
        }
    }

    public int Progress
    {
        get
        {
            return progress;
        }
        set
        {
            progress = value;
            if (progress < progressSprites.Length)
            {
                progressImage.sprite = progressSprites[progress];
            }
            else
            {
                progress = progressSprites.Length - 1;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mascotOnScreen = true;
        setNumber = Constants.Level4.currentSet;
        Progress = Constants.Level4.progress;
        rewardsText.text = Constants.rewards.ToString();
        NonInteractableBoxParent.transform.GetChild(4).GetComponent<IDs>().id = upperImages[0].upperImages.transform.GetChild(0).gameObject.name;
        NonInteractableBoxParent.transform.GetChild(5).GetComponent<IDs>().id = upperImages[0].upperImages.transform.GetChild(1).gameObject.name;
        NonInteractableBoxParent.transform.GetChild(6).GetComponent<IDs>().id = upperImages[0].upperImages.transform.GetChild(2).gameObject.name;
        NonInteractableBoxParent.transform.GetChild(7).GetComponent<IDs>().id = upperImages[0].upperImages.transform.GetChild(3).gameObject.name;
        if (Constants.Level4.gameState == Constants.State.NORMAL)
        {
            Constants.Level4.nextLevel = 2;
        }
        if (Constants.Level4.canSkip)
        {
            skipButton.gameObject.SetActive(true);
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(SkipButtonClicked);
        }
        correctAnswerStreak = 0;
        currentShapes = 0;
        setOfUpperImages = 0;
        initMascotPos1 = mascot.rect.anchoredPosition;
        Tutorial = true;
        backButton.interactable = false;
        co = StartCoroutine(MascotAppear());
        
    }

    IEnumerator MascotAppear()
    {
        yield return oneSec;
        yield return oneSec;
        mascot.rect.eulerAngles = Vector3.zero;
        yield return oneSec;
        AudioManager.instance.DecreaseBackgroundMusicVolume();
        mascot.SpeechBubbleRight();
        onoff = true;
        EnableSpeaking();
        yield return new WaitForSeconds(mascot.PlayClip(0));
        DisableSpeaking();
        onoff = false;
        yield return oneSec;
        mascot.rect.DOAnchorPos(new Vector2(344f, -100f), 2f);
        mascot.rect.DOScale(1f, 2f);
        yield return oneSec;
        yield return oneSec;
        mascot.SpeechBubbleLeft();
        onoff = true;
        mascot.ChangeMascotImage();
        NonInteractableBoxParent.SetActive(true);
        EnableSpeaking();
        yield return new WaitForSeconds(mascot.PlayClip(1));
        onoff = false;
        mascot.MascotDisappear();
        DisableSpeaking();
        yield return oneSec;
        InteractaleObjectsParent.SetActive(true);
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].gameObject.GetComponent<DraggableObjects>().enabled = false;
        }
        InteractaleShapesParent.SetActive(true);
        yield return new WaitForSeconds(mascot.PlayClip(2));
        handGameObject.SetActive(true);
        yield return oneSec;
        hand.DOAnchorPos(new Vector2(-231f, -193f), 2f);
        yield return oneSec;
        yield return oneSec;
        shapes[0].gameObject.transform.parent = canvas2.transform;
        handGameObject.transform.parent = canvas2.transform;
        hand.DOAnchorPos(new Vector2(130f, -60f), 2f);
        shapes[0].rectTransform.DOAnchorPos(new Vector2(93f, -21f), 2f);
        yield return oneSec;
        yield return oneSec;
        AudioManager.instance.Play("CorrectAnswer");
        shapes[0].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        yield return oneSec;
        hand.DOAnchorPos(new Vector2(-70f, -193f), 2f);
        yield return oneSec;
        handGameObject.transform.parent = canvas.transform;
        yield return oneSec;
        shapes[1].gameObject.transform.parent = canvas2.transform;
        handGameObject.transform.parent = canvas2.transform;
        hand.DOAnchorPos(new Vector2(305f, -58f), 2f);
        shapes[1].rectTransform.DOAnchorPos(new Vector2(270f, -21f), 2f);
        yield return oneSec;
        yield return oneSec;
        AudioManager.instance.Play("CorrectAnswer");
        shapes[1].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        yield return oneSec;
        hand.DOAnchorPos(new Vector2(136f, -193f), 2f);
        yield return oneSec;
        handGameObject.transform.parent = canvas.transform;
        yield return oneSec;
        shapes[2].gameObject.transform.parent = canvas2.transform;
        handGameObject.transform.parent = canvas2.transform;
        hand.DOAnchorPos(new Vector2(-230f, -58f), 2f);
        shapes[2].rectTransform.DOAnchorPos(new Vector2(-279f, -21f), 2f);
        yield return oneSec;
        yield return oneSec;
        AudioManager.instance.Play("CorrectAnswer");
        shapes[2].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        yield return oneSec;
        hand.DOAnchorPos(new Vector2(318f, -193f), 2f);
        yield return oneSec;
        handGameObject.transform.parent = canvas.transform;
        yield return oneSec;
        shapes[3].gameObject.transform.parent = canvas2.transform;
        handGameObject.transform.parent = canvas2.transform;
        hand.DOAnchorPos(new Vector2(-42f, -58f), 2f);
        shapes[3].rectTransform.DOAnchorPos(new Vector2(-94f, -21f), 2f);
        yield return oneSec;
        yield return oneSec;
        AudioManager.instance.Play("CorrectAnswer");
        shapes[3].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        yield return oneSec;
        for(int i = 0; i< shapes.Length; i++)
		{
            shapes[i].transform.parent = InteractaleObjectsParent.transform;
		}
        InteractaleObjectsParent.SetActive(false);
        InteractaleShapesParent.SetActive(false);
        hand.DOAnchorPos(new Vector2(0, 0), 2f);
        handGameObject.transform.parent = canvas.transform;
        yield return oneSec;
        hand.DOAnchorPos(new Vector2(392f, 111f), 2f);
        AudioManager.instance.Play("ReplayAudio");
        audioReplayImage.gameObject.SetActive(true);
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        handGameObject.SetActive(false);
        mascot.MascotReappear();
        onoff = true;
        EnableSpeaking();
        yield return new WaitForSeconds(mascot.PlayClip(4));
        DisableSpeaking();
        mascot.MascotDisappear();
        yield return oneSec;
        NonInteractableBoxParent.SetActive(false);
        presentationEndIcons.SetActive(true);
        presentationEndIcons.transform.GetChild(0).GetComponent<Button>().enabled = false;
        presentationEndIcons.transform.GetChild(1).GetComponent<Button>().enabled = false;
        AudioManager.instance.Play("PresentationCompletionInstructions");
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        AudioManager.instance.Stop("BackgroundMusic");
        skipButton.gameObject.SetActive(false);
        presentationEndIcons.transform.GetChild(0).GetComponent<Button>().enabled = true;
        presentationEndIcons.transform.GetChild(1).GetComponent<Button>().enabled = true;
        shapes[0].rectTransform.DOAnchorPos(new Vector2(-279f, -157f), 0f);
        shapes[1].rectTransform.DOAnchorPos(new Vector2(-94f, -157f), 0f);
        shapes[2].rectTransform.DOAnchorPos(new Vector2(93f, -157f), 0f);
        shapes[3].rectTransform.DOAnchorPos(new Vector2(270f, -157f), 0f);
        for(int i = 0; i < shapes.Length; i++)
		{
            shapes[i].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
		}
    }

    public void PresentationPlayAgain()
    {
        musicButton.image.sprite = buttonImage;
        AudioManager.instance.Play("BackgroundMusic");
        presentationEndIcons.SetActive(false);
        audioReplay.gameObject.SetActive(false);
        hand.DOAnchorPos(new Vector2(0f, 0f), 0f);
        mascot.rect.DOAnchorPos(new Vector2(0f, -90f), 0f);
        mascot.mascotImage.sprite = mascot.mascotNormalSprite;
        mascot.gameObject.SetActive(true);
        mascot.SpeechBubbleRight();
        mascot.transform.GetChild(0).gameObject.SetActive(false);
        mascot.transform.GetChild(1).gameObject.SetActive(false);
        shapes[0].rectTransform.DOAnchorPos(new Vector2(-279f, -157f), 0f);
        shapes[1].rectTransform.DOAnchorPos(new Vector2(-94f, -157f), 0f);
        shapes[2].rectTransform.DOAnchorPos(new Vector2(93f, -157f), 0f);
        shapes[3].rectTransform.DOAnchorPos(new Vector2(270f, -157f), 0f);
        StartCoroutine(MascotAppear());
        skipButton.gameObject.SetActive(true);
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(SkipPresentationButtonClicked);
    }

    public void PlayGame()
	{
        StartCoroutine(Play());
	}

    IEnumerator Play()
	{
        musicButton.image.sprite = buttonImage;
        presentationEndIcons.SetActive(false);
        AudioManager.instance.Play("BackgroundMusic");
        yield return oneSec;
        if (setNumber == 0)
		{
            if(currentShapes == 0)
			{
                NonInteractableBoxParent.SetActive(true);
                InteractaleObjectsParent.SetActive(true);
                upperImages[0].upperImages.gameObject.SetActive(true);
                upperImages[0].upperImages.transform.GetChild(0).gameObject.SetActive(true);
                upperImages[0].upperImages.transform.GetChild(1).gameObject.SetActive(true);
                upperImages[0].upperImages.transform.GetChild(2).gameObject.SetActive(true);
                upperImages[0].upperImages.transform.GetChild(3).gameObject.SetActive(true);
                yield return new WaitForSeconds(mascot.PlayClip(2));
                for (int i = 0; i < shapes.Length; i++)
                {
                    shapes[i].gameObject.GetComponent<DraggableObjects>().enabled = true;
                }
                backButton.interactable = true;
                hintButton.interactable = true;
            }
        }
        
    }

    public void DisableSpeaking()
    {
        if (mascotOnScreen)
        {
            mascot.speechDotsRect.gameObject.SetActive(false);
            mascot.speechBubble.gameObject.SetActive(false);
        }
    }

    public void EnableSpeaking()
    {
        if (mascotOnScreen)
        {
            mascot.speechBubble.gameObject.SetActive(true);
            mascot.speechDotsRect.gameObject.SetActive(true);

        }
    }

    public void SkipButtonClicked()
    {
        Time.timeScale = 0f;
        MenuButtonsOnScreen = true;
        AudioManager.instance.PauseAudio();
        AudioManager.instance.PlayMenuAudio("SkipAudio");

        CanSelectBox = false;
        skipButton.gameObject.SetActive(false);
        skipButtonIcons.SetActive(true);
    }

    public void SkipContinueClicked()
    {
        Time.timeScale = 1f;
        MenuButtonsOnScreen = false;
        AudioManager.instance.Stop("SkipAudio");
        AudioManager.instance.ContinueAudio();
        CanSelectBox = true;
        skipButton.gameObject.SetActive(true);
        skipButtonIcons.SetActive(false);
    }

    public void PlayAgain()
	{
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        Time.timeScale = 1f;
        AudioManager.instance.Play("BackgroundMusic");
        Constants.Level1.firstTimeLevel = false;
        level1.StopAllCoroutines();
        level1.NextShapes();
    }

    public void SkipPresentationButtonClicked()
	{
        Time.timeScale = 0f;
        AudioManager.instance.PauseAudio();
        AudioManager.instance.Play("PresentationSkip");
        skipButton.gameObject.SetActive(false);
        presentationSkipButtonIcons.SetActive(true);
	}

    public void PresentationSkipContinueClicked()
    {
        Time.timeScale = 1f;
        MenuButtonsOnScreen = false;
        AudioManager.instance.Stop("PresentationSkip");
        AudioManager.instance.ContinueAudio();
        skipButton.gameObject.SetActive(true);
        presentationSkipButtonIcons.SetActive(false);
    }

    public void PresentationSkipExitClicked()
    {
        StopAllCoroutines();
        AudioManager.instance.StopAllAudioExceptBackground();
        Time.timeScale = 1f;
        MenuButtonsOnScreen = false;
        presentationSkipButtonIcons.SetActive(false);
        StartCoroutine(PresentationSkipAnimation());
    }

    IEnumerator PresentationSkipAnimation()
	{
        musicButton.image.sprite = buttonImage;
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(SkipButtonClicked);
        if (Constants.Level4.canSkip)
        {
            skipButton.gameObject.SetActive(true);
        }
        else
        {
            skipButton.gameObject.SetActive(false);

        }
        hand.gameObject.SetActive(false);
        for(int i = 0; i < upperImages.Count; i++)
		{
            upperImages[i].upperImages.gameObject.SetActive(false);
		}
        mascot.MascotDisappear();
        mascotOnScreen = false;
        Tutorial = false;
        yield return oneSec;
        NonInteractableBoxParent.SetActive(true);
        InteractaleObjectsParent.SetActive(true);
        audioReplay.gameObject.SetActive(true);
        audioReplay.enabled = true;
        CanSelectBox = false;
        PlayerPrefs.GetInt("SetNumber");
        PlayerPrefs.GetInt("SetCurrentSet");
        SetNumberData();
    }

    public void SetNumberData()
	{
        if (PlayerPrefs.GetInt("SetCurrentSet") == 0)
        {
            audioReplay.interactable = true;
            upperImages[0].upperImages.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetCurrentSet") == 1)
        {
            audioReplay.interactable = true;
            upperImages[1].upperImages.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetCurrentSet") == 2)
        {
            audioReplay.interactable = true;
            upperImages[2].upperImages.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetCurrentSet") == 3)
        {
            audioReplay.interactable = true;
            upperImages[3].upperImages.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetCurrentSet") == 4)
        {
            audioReplay.interactable = true;
            upperImages[4].upperImages.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetCurrentSet") == 5)
        {
            audioReplay.interactable = true;
            upperImages[5].upperImages.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetCurrentSet") == 6)
        {
            audioReplay.interactable = true;
            upperImages[6].upperImages.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetCurrentSet") == 7)
        {
            audioReplay.interactable = true;
            upperImages[7].upperImages.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetCurrentSet") == 8)
        {
            audioReplay.interactable = true;
            upperImages[8].upperImages.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetCurrentSet") == 9)
        {
            audioReplay.interactable = true;
            upperImages[9].upperImages.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetCurrentSet") == 10)
        {
            audioReplay.interactable = true;
            upperImages[10].upperImages.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetCurrentSet") == 11)
        {
            audioReplay.interactable = true;
            upperImages[11].upperImages.gameObject.SetActive(true);
        }
	}

    public void NextLevel()
    {
        StopAllCoroutines();
        KillAllTweens();
        Time.timeScale = 1f;
        SceneLoader.instance.LoadNextLevel(Constants.Level4.nextLevel);
    }

    private void KillAllTweens()
    {
       
        hand.DOKill();
        handImage.DOKill();
        hand.transform.DOKill();
        mascot.KillAllTweens();
    }

    public void ReplayLevel()
    {
        levelCompletedIcons.SetActive(false);
        AudioManager.instance.Stop("BackgroundMusic");
        AudioManager.instance.Play("BackgroundMusic");
        audioReplay.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(true);
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(SkipButtonClicked);
        mascot.rect.DOAnchorPos(new Vector2(0f, -90f), 0f);
        mascot.mascotImage.sprite = mascot.mascotNormalSprite;
        mascot.gameObject.SetActive(true);
        hand.DOAnchorPos(new Vector2(0f, 0f), 0f);
        StartCoroutine(MascotAppear());
    }

    public void ReplayAudioInstructions()
    {
        StartCoroutine(AudioInstructionsRepeatCoroutine());
    }

    private IEnumerator AudioInstructionsRepeatCoroutine()
    {
        audioReplay.interactable = false;
        AudioManager.instance.DecreaseBackgroundMusicVolume();
        ShapeDraggingManager.canDrag = false;
        yield return new WaitForSeconds(mascot.PlayClip(2));
        ShapeDraggingManager.canDrag = true;
        AudioManager.instance.IncreaseBackgroundMusicVolume();
        audioReplay.interactable = true;
    }

    public void HomeButtonClicked()
    {
        Time.timeScale = 0f;
        MenuButtonsOnScreen = true;
        AudioManager.instance.PauseAudio();
        ShapeDraggingManager.canDrag = false;
        AudioManager.instance.PlayMenuAudio("HomeButtonAudio");
        homeButtonIcons.SetActive(true);
        levelCompletedIcons.SetActive(false);
        presentationEndIcons.SetActive(false);
        presentationSkipButtonIcons.SetActive(false);
        skipButtonIcons.SetActive(false);
    }

    public void HomeContinueClicked()
    {
        Time.timeScale = 1f;
        MenuButtonsOnScreen = false;
        AudioManager.instance.Stop("HomeButtonAudio");
        AudioManager.instance.ContinueAudio();
        ShapeDraggingManager.canDrag = true;
        homeButtonIcons.SetActive(false);
    }

    public void ToggleBackgroundMusic()
    {
        AudioMute = !AudioMute;
    }

    public void BackButtonClicked()
    {
        Time.timeScale = 0f;
        MenuButtonsOnScreen = true;
        AudioManager.instance.PauseAudio();
        CanSelectBox = false;
        AudioManager.instance.PlayMenuAudio("HomeButtonAudio");
        backButtonIcons.SetActive(true);
    }

    public void PreviousLevelClicked()
    {
        Time.timeScale = 0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
        Time.timeScale = 1f;
        Level3Manager.instance.skipButton.gameObject.SetActive(true);
        Level3Manager.instance.skipButton.onClick.RemoveAllListeners();
        Level3Manager.instance.skipButton.onClick.AddListener(Level3Manager.instance.SkipButtonClicked);
        PlayerPrefs.GetInt("LevelBar");
        PlayerPrefs.GetString("Score");
    }

    public void StartPresentation()
    {
        PlayerPrefs.SetInt("SetNumber", setNumber);
        PlayerPrefs.SetInt("SetCurrentSet", currentShapes);
        hintButton.interactable = false;
        musicButton.image.sprite = buttonImage;
        AudioManager.instance.Stop("BackgroundMusic");
        AudioManager.instance.Play("BackgroundMusic");
        for(int i = 0; i < upperImages.Count; i++)
		{
            upperImages[i].upperImages.gameObject.SetActive(false);
		}
        audioReplay.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(true);
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(SkipPresentationButtonClicked);
        NonInteractableBoxParent.SetActive(false);
        InteractaleObjectsParent.SetActive(false);
        mascot.rect.DOAnchorPos(new Vector2(0f, -90f), 0f);
        mascot.mascotImage.sprite = mascot.mascotNormalSprite;
        mascot.gameObject.SetActive(true);
        mascot.SpeechBubbleRight();
        hand.DOAnchorPos(new Vector2(0f, 0f), 0f);
        shapes[0].rectTransform.DOAnchorPos(new Vector2(-279f, -157f), 0f);
        shapes[1].rectTransform.DOAnchorPos(new Vector2(-94f, -157f), 0f);
        shapes[2].rectTransform.DOAnchorPos(new Vector2(93f, -157f), 0f);
        shapes[3].rectTransform.DOAnchorPos(new Vector2(270f, -157f), 0f);
        StartCoroutine(MascotAppear());
        presentationEndIcons.gameObject.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        presentationEndIcons.gameObject.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(PlayWithSavedData);
    }

    public void PlayWithSavedData()
    {
        AudioManager.instance.Play("BackgroundMusic");
        skipButton.gameObject.SetActive(false);
        presentationEndIcons.SetActive(false);
        hintButton.interactable = true;
        NonInteractableBoxParent.SetActive(true);
        InteractaleObjectsParent.SetActive(true);
        shapes[0].gameObject.SetActive(true);
        shapes[1].gameObject.SetActive(true);
        shapes[2].gameObject.SetActive(true);
        shapes[3].gameObject.SetActive(true);
        shapes[0].gameObject.GetComponent<DraggableObjects>().enabled = true;
        shapes[1].gameObject.GetComponent<DraggableObjects>().enabled = true;
        shapes[2].gameObject.GetComponent<DraggableObjects>().enabled = true;
        shapes[3].gameObject.GetComponent<DraggableObjects>().enabled = true;
        PlayerPrefs.GetInt("SetNumber");
        PlayerPrefs.GetInt("SetCurrentSet");
        SetNumberData();
    }

    // Update is called once per frame
    void Update()
    {
        if (onoff)
        {
            speechbubble.gameObject.SetActive(true);
            speechDots.gameObject.SetActive(true);
        }
        else
        {
            speechbubble.gameObject.SetActive(false);
            speechDots.gameObject.SetActive(false);
        }
    }

    public void GoToHomeScreen()
    {
        AudioManager.instance.Stop("BackgroundMusic");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
