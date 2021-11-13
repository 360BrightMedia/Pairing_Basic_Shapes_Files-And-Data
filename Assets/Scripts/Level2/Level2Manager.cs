using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Level2Manager : MonoBehaviour
{
    public static Level2Manager instance;

    [SerializeField] private int setNumber;
    public int currentSet;
    [SerializeField] private Level0MascotManager mascot;
    [SerializeField] private Sprite[] triangleSprites;
    [SerializeField] private Sprite[] squareSprites;
    [SerializeField] private Sprite[] rectangleSprites;
    [SerializeField] private Sprite[] circleSprites;
    [SerializeField] private Image[] shapes;
    [SerializeField] private Image[] boxesAndLines;
    [SerializeField] private RectTransform hand;
    [SerializeField] private GameObject boxParent;
    [SerializeField] private Image handImage;
    [SerializeField] private GameObject confetti;
    [SerializeField] private GameObject levelCompletedIcons;
    [SerializeField] private Button audioReplay;
    [SerializeField] private Image progressImage;
    [SerializeField] private Sprite[] progressSprites;
    [SerializeField] private TextMeshProUGUI rewardsText;
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
    public Image speechBubbleImage;
    public Image speechDotsImage;
    public Image shapeImage;
    public GameObject handGameObject;
    public Image[] tickImage;
    public Image audioReplayImage;
    public GameObject[] sets;
    public List<UpperShapes> upperShapes = new List<UpperShapes>();
    public List<Shapes> shape = new List<Shapes>();
    int numberOfCircles;
    int numberOfSquares;
    int numberOfRectangles;
    int numberOfTriangles;
    int numberOfCircles2;
    int numberOfSquares2;
    int numberOfRectangles2;
    int numberOfTriangles2;
    int numberOfSquares3;
    int numberOfRectangles3;
    public Image[] progressImages;
    public Sprite buttonImage;
    public Sprite newButtonImage;
    public Button musicButton;
    public Button backButton;
    public GameObject backButtonGameObject;

    private void Awake()
    {
        instance = this;
        //HandManagerLV1.selectedShape += CheckAnswer;
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

    void ChangeButtonImage()
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

    void Start()
    {
        mascotOnScreen = true;
        setNumber = 0;
        Progress = Constants.Level2.progress;
        rewardsText.text = Constants.rewards.ToString();
        numberOfCircles = 0;
        numberOfRectangles = 0;
        numberOfSquares = 0;
        numberOfTriangles = 0;
        numberOfTriangles2 = 0;
        numberOfCircles2 = 0;
        numberOfRectangles2 = 0;
        numberOfSquares2 = 0;
        numberOfSquares3 = 0;
        numberOfRectangles3 = 0;
        if (Constants.Level2.gameState == Constants.State.NORMAL)
        {
            Constants.Level2.nextLevel = 4;
        }
        if (Constants.Level2.canSkip)
        {
            skipButton.gameObject.SetActive(true);
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(SkipButtonClicked);
        }
        correctAnswerStreak = 0;
        currentSet = 0;
        initMascotPos1 = mascot.rect.anchoredPosition;
        Tutorial = true;
        StartCoroutine(MascotAppear());
    }

    private IEnumerator MascotAppear()
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
        mascot.rect.DOAnchorPos(new Vector3(344f, -90f), 2f);
        mascot.rect.DOScale(1f, 2f);

        yield return oneSec;
        yield return oneSec;
        boxParent.SetActive(true);
        shapeImage.gameObject.SetActive(true);
        mascot.SpeechBubbleLeft();
        onoff = true;
        mascot.ChangeMascotImage();
        EnableSpeaking();
        yield return new WaitForSeconds(mascot.PlayClip(1));
        mascot.MascotDisappear();
        yield return oneSec;
        for(int i = 0; i < shapes.Length; i++)
		{
            shapes[i].gameObject.SetActive(true);
		}
        handGameObject.SetActive(true);
        hand.DOAnchorPos(new Vector2(-264f, -112f), 2f);
        yield return oneSec;
        yield return oneSec;
        tickImage[0].gameObject.SetActive(true);
        AudioManager.instance.Play("CorrectAnswer");
        yield return oneSec;
        hand.DOAnchorPos(new Vector2(39f, -225f), 2f);
        yield return oneSec;
        yield return oneSec;
        tickImage[1].gameObject.SetActive(true);
        AudioManager.instance.Play("CorrectAnswer");
        yield return oneSec;
        hand.DOAnchorPos(new Vector2(203f, -225f), 2f);
        yield return oneSec;
        yield return oneSec;
        tickImage[2].gameObject.SetActive(true);
        AudioManager.instance.Play("CorrectAnswer");
        yield return oneSec;
        hand.DOAnchorPos(new Vector2(340f, -111f), 2f);
        yield return oneSec;
        yield return oneSec;
        tickImage[3].gameObject.SetActive(true);
        AudioManager.instance.Play("CorrectAnswer");
        yield return oneSec;
        yield return oneSec;
        for(int i = 0; i < tickImage.Length; i++)
		{
            tickImage[i].gameObject.SetActive(false);
		}
        shapes[1].gameObject.SetActive(false);
        shapes[2].gameObject.SetActive(false);
        shapes[3].gameObject.SetActive(false);
        shapes[5].gameObject.SetActive(false);
        shapes[6].gameObject.SetActive(false);
        shapes[9].gameObject.SetActive(false);
        handGameObject.SetActive(false);
        yield return oneSec;
        yield return oneSec;
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].gameObject.SetActive(false);
        }
        shapeImage.gameObject.SetActive(false);
        mascot.MascotReappear();
        EnableSpeaking();
        yield return new WaitForSeconds(mascot.PlayClip(2));
        hand.DOAnchorPos(new Vector2(0f, 0f), 2f);
        yield return oneSec;
        yield return oneSec;
        audioReplayImage.gameObject.SetActive(true);
        audioReplay.GetComponent<Button>().enabled = false;
        handGameObject.SetActive(true);
        hand.DOAnchorPos(new Vector2(394f, 103f), 2f);
        yield return oneSec;
        AudioManager.instance.Play("ReplayAudio");
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        handGameObject.SetActive(false);
        mascot.MascotDisappear();
        yield return oneSec;
        setNumber = 0;
        sets[0].gameObject.SetActive(true);
        for(int i = 0; i < shape.Count; i++)
		{
            for(int j = 0; j < shape[i].image.Length; j++)
			{
                shape[0].image[j].GetComponent<Button>().enabled = false;
			}
		}
        skipButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(mascot.PlayClip(1));
        backButton.interactable = true;
        hintButton.interactable = true;
        audioReplay.GetComponent<Button>().enabled = true;
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[0].image[j].GetComponent<Button>().enabled = true;
            }
        }
    }

    public void Set1(Button button)
	{
        GameObject set1Objects = button.gameObject;
        string nameOfCircles = set1Objects.GetComponent<Image>().sprite.name;
        if(nameOfCircles == upperShapes[0].id)
		{
            AudioManager.instance.DecreaseBackgroundMusicVolume();
            AudioManager.instance.Play("CorrectAnswer");
            Time.timeScale = 0f;
            AudioManager.instance.IncreaseBackgroundMusicVolume();
            set1Objects.transform.GetChild(0).gameObject.SetActive(true);
            numberOfCircles++;
            set1Objects.GetComponent<Button>().enabled = false;
            if (numberOfCircles == 3)
            {
                Debug.Log(numberOfCircles);
                progressImages[0].gameObject.SetActive(true);
                setNumber++;
                for (int i = 0; i < shape.Count; i++)
                {
                    for (int j = 0; j < shape[i].image.Length; j++)
                    {
                        shape[0].image[j].gameObject.GetComponent<Button>().enabled = false;
                    }
                }
                StartCoroutine(WaitForTwoSeconds1());
            }
        }
		else
		{
            AudioManager.instance.Play("WrongAnswer");
            set1Objects.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(WrongAnswerMusic());
		}
    }

    public void Set2(Button button1)
	{
        GameObject set2Objects = button1.gameObject;
        string nameOfTriangles = set2Objects.GetComponent<Image>().sprite.name;
        if (nameOfTriangles == upperShapes[1].id)
        {
            AudioManager.instance.DecreaseBackgroundMusicVolume();
            AudioManager.instance.Play("CorrectAnswer");
            AudioManager.instance.IncreaseBackgroundMusicVolume();
            Debug.Log(AudioManager.instance.Play("CorrectAnswer"));
            set2Objects.transform.GetChild(0).gameObject.SetActive(true);
            numberOfTriangles++;
            set2Objects.GetComponent<Button>().enabled = false;
            if (numberOfTriangles == 2)
            {
                progressImages[1].gameObject.SetActive(true);
                setNumber++;
                for (int i = 0; i < shape.Count; i++)
                {
                    for (int j = 0; j < shape[i].image.Length; j++)
                    {
                        shape[1].image[j].gameObject.GetComponent<Button>().enabled = false;
                    }
                }
                StartCoroutine(WaitForTwoSeconds2());
            }
        }
        else
        {
            AudioManager.instance.Play("WrongAnswer");
            set2Objects.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(WrongAnswerMusic());
        }
    }
    public void Set3(Button button2)
	{
        GameObject set3Objects = button2.gameObject;
        string nameOfRectangles = set3Objects.GetComponent<Image>().sprite.name;
        if (nameOfRectangles == upperShapes[2].id)
        {
            AudioManager.instance.DecreaseBackgroundMusicVolume();
            AudioManager.instance.Play("CorrectAnswer");
            AudioManager.instance.IncreaseBackgroundMusicVolume();
            Debug.Log(AudioManager.instance.Play("CorrectAnswer"));
            set3Objects.transform.GetChild(0).gameObject.SetActive(true);
            numberOfRectangles++;
            set3Objects.GetComponent<Button>().enabled = false;
            if (numberOfRectangles == 2)
            {
                progressImages[2].gameObject.SetActive(true);
                setNumber++;
                for (int i = 0; i < shape.Count; i++)
                {
                    for (int j = 0; j < shape[i].image.Length; j++)
                    {
                        shape[2].image[j].gameObject.GetComponent<Button>().enabled = false;
                    }
                }
                StartCoroutine(WaitForTwoSeconds3());
            }
        }
        else
        {
            AudioManager.instance.Play("WrongAnswer");
            set3Objects.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(WrongAnswerMusic());
        }
    }

    public void Set4(Button button3)
	{
        GameObject set4Objects = button3.gameObject;
        string nameOfSquares = set4Objects.GetComponent<Image>().sprite.name;
        if (nameOfSquares == upperShapes[3].id)
        {
            AudioManager.instance.DecreaseBackgroundMusicVolume();
            AudioManager.instance.Play("CorrectAnswer");
            AudioManager.instance.IncreaseBackgroundMusicVolume();
            Debug.Log(AudioManager.instance.Play("CorrectAnswer"));
            set4Objects.transform.GetChild(0).gameObject.SetActive(true);
            numberOfSquares++;
            set4Objects.GetComponent<Button>().enabled = false;
            if (numberOfSquares == 3)
            {
                progressImages[3].gameObject.SetActive(true);
                setNumber++;
                for (int i = 0; i < shape.Count; i++)
                {
                    for (int j = 0; j < shape[i].image.Length; j++)
                    {
                        shape[3].image[j].gameObject.GetComponent<Button>().enabled = false;
                    }
                }
                StartCoroutine(WaitForTwoSeconds4());
            }
        }
        else
        {
            AudioManager.instance.Play("WrongAnswer");
            set4Objects.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(WrongAnswerMusic());
        }
    }

    public void Set5(Button button4)
	{
        GameObject set5Objects = button4.gameObject;
        string triangles2 = set5Objects.GetComponent<Image>().sprite.name;
        if (triangles2 == upperShapes[4].id)
        {
            AudioManager.instance.DecreaseBackgroundMusicVolume();
            AudioManager.instance.Play("CorrectAnswer");
            AudioManager.instance.IncreaseBackgroundMusicVolume();
            Debug.Log(AudioManager.instance.Play("CorrectAnswer"));
            set5Objects.transform.GetChild(0).gameObject.SetActive(true);
            numberOfTriangles2++;
            set5Objects.GetComponent<Button>().enabled = false;
            if (numberOfTriangles2 == 2)
            {
                progressImages[4].gameObject.SetActive(true);
                setNumber++;
                for (int i = 0; i < shape.Count; i++)
                {
                    for (int j = 0; j < shape[i].image.Length; j++)
                    {
                        shape[4].image[j].gameObject.GetComponent<Button>().enabled = false;
                    }
                }
                StartCoroutine(WaitForTwoSeconds5());
            }
        }
        else
        {
            AudioManager.instance.Play("WrongAnswer");
            set5Objects.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(WrongAnswerMusic());
        }
    }

    public void Set6(Button button5)
	{
        GameObject set6Objects = button5.gameObject;
        string circle2 = set6Objects.GetComponent<Image>().sprite.name;
        if (circle2 == upperShapes[5].id)
        {
            AudioManager.instance.DecreaseBackgroundMusicVolume();
            AudioManager.instance.Play("CorrectAnswer");
            AudioManager.instance.IncreaseBackgroundMusicVolume();
            Debug.Log(AudioManager.instance.Play("CorrectAnswer"));
            set6Objects.transform.GetChild(0).gameObject.SetActive(true);
            numberOfCircles2++;
            set6Objects.GetComponent<Button>().enabled = false;
            if (numberOfCircles2 == 3)
            {
                progressImages[5].gameObject.SetActive(true);
                setNumber++;
                for (int i = 0; i < shape.Count; i++)
                {
                    for (int j = 0; j < shape[i].image.Length; j++)
                    {
                        shape[5].image[j].gameObject.GetComponent<Button>().enabled = false;
                    }
                }
                StartCoroutine(WaitForTwoSeconds6());
            }
        }
        else
        {
            AudioManager.instance.Play("WrongAnswer");
            set6Objects.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(WrongAnswerMusic());
        }
    }

    public void Set7(Button button6)
	{
        GameObject set7Objects = button6.gameObject;
        string rectangle2 = set7Objects.GetComponent<Image>().sprite.name;
        if (rectangle2 == upperShapes[6].id)
        {
            AudioManager.instance.DecreaseBackgroundMusicVolume();
            AudioManager.instance.Play("CorrectAnswer");
            AudioManager.instance.IncreaseBackgroundMusicVolume();
            Debug.Log(AudioManager.instance.Play("CorrectAnswer"));
            set7Objects.transform.GetChild(0).gameObject.SetActive(true);
            numberOfRectangles2++;
            set7Objects.GetComponent<Button>().enabled = false;
            if (numberOfRectangles2 == 2)
            {
                progressImages[6].gameObject.SetActive(true);
                setNumber++;
                for (int i = 0; i < shape.Count; i++)
                {
                    for (int j = 0; j < shape[i].image.Length; j++)
                    {
                        shape[6].image[j].gameObject.GetComponent<Button>().enabled = false;
                    }
                }
                StartCoroutine(WaitForTwoSeconds7());
            }
        }
        else
        {
            AudioManager.instance.Play("WrongAnswer");
            set7Objects.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(WrongAnswerMusic());
        }
    }

    public void Set8(Button button7)
	{
        GameObject set8Objects = button7.gameObject;
        string squares2 = set8Objects.GetComponent<Image>().sprite.name;
        if (squares2 == upperShapes[7].id)
        {
            AudioManager.instance.DecreaseBackgroundMusicVolume();
            AudioManager.instance.Play("CorrectAnswer");
            AudioManager.instance.IncreaseBackgroundMusicVolume();
            Debug.Log(AudioManager.instance.Play("CorrectAnswer"));
            set8Objects.transform.GetChild(0).gameObject.SetActive(true);
            numberOfSquares2++;
            set8Objects.GetComponent<Button>().enabled = false;
            if (numberOfSquares2 == 3)
            {
                progressImages[7].gameObject.SetActive(true);
                setNumber++;
                for (int i = 0; i < shape.Count; i++)
                {
                    for (int j = 0; j < shape[i].image.Length; j++)
                    {
                        shape[7].image[j].gameObject.GetComponent<Button>().enabled = false;
                    }
                }
                StartCoroutine(WaitForTwoSeconds8());
            }
        }
        else
        {
            AudioManager.instance.Play("WrongAnswer");
            set8Objects.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(WrongAnswerMusic());
        }
    }

    public void Set9(Button button8)
	{
        GameObject set9Objects = button8.gameObject;
        string squares3 = set9Objects.GetComponent<Image>().sprite.name;
        if (squares3 == upperShapes[8].id)
        {
            AudioManager.instance.DecreaseBackgroundMusicVolume();
            AudioManager.instance.Play("CorrectAnswer");
            AudioManager.instance.IncreaseBackgroundMusicVolume();
            Debug.Log(AudioManager.instance.Play("CorrectAnswer"));
            set9Objects.transform.GetChild(0).gameObject.SetActive(true);
            numberOfSquares3++;
            set9Objects.GetComponent<Button>().enabled = false;
            if (numberOfSquares3 == 3)
            {
                progressImages[8].gameObject.SetActive(true);
                setNumber++;
                for (int i = 0; i < shape.Count; i++)
                {
                    for (int j = 0; j < shape[i].image.Length; j++)
                    {
                        shape[8].image[j].gameObject.GetComponent<Button>().enabled = false;
                    }
                }
                StartCoroutine(WaitForTwoSeconds9());
            }
        }
        else
        {
            AudioManager.instance.Play("WrongAnswer");
            set9Objects.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(WrongAnswerMusic());
        }
    }

    public void Set10(Button button9)
	{
        GameObject set10Objects = button9.gameObject;
        string rectangles3 = set10Objects.GetComponent<Image>().sprite.name;
        if (rectangles3 == upperShapes[9].id)
        {
            AudioManager.instance.DecreaseBackgroundMusicVolume();
            AudioManager.instance.Play("CorrectAnswer");
            AudioManager.instance.IncreaseBackgroundMusicVolume();
            Debug.Log(AudioManager.instance.Play("CorrectAnswer"));
            set10Objects.transform.GetChild(0).gameObject.SetActive(true);
            numberOfRectangles3++;
            set10Objects.GetComponent<Button>().enabled = false;
            if (numberOfRectangles3 == 2)
            {
                progressImages[9].gameObject.SetActive(true);
                setNumber++;
                for (int i = 0; i < shape.Count; i++)
                {
                    for (int j = 0; j < shape[i].image.Length; j++)
                    {
                        shape[9].image[j].gameObject.GetComponent<Button>().enabled = false;
                    }
                }
                StartCoroutine(WaitForTwoSeconds10());
            }
        }
        else
        {
            AudioManager.instance.Play("WrongAnswer");
            set10Objects.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(WrongAnswerMusic());
        }
    }

    public void PresentationSkipButtonClicked()
    {
        Time.timeScale = 0f;
        MenuButtonsOnScreen = true;
        AudioManager.instance.PauseAudio();
        AudioManager.instance.PlayMenuAudio("PresentationSkip");
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

   
    private void BoxesSetActiveFirstTime()
    {
        boxParent.gameObject.SetActive(true);
        for(int i = 0; i < boxesAndLines.Length; i++)
        {
            boxesAndLines[i].DOFade(1f, 1f).From(0f).SetEase(Ease.Linear);
        }
    }



    /*public void PresentationPlayAgain()
    {
        MenuButtonsOnScreen = false;
        presentationEndIcons.SetActive(false);
        StartPresentation(false);
    }

    public void PresentationPlayGame()
    {
        if (presentationCoroutine != null)
        {
            StopCoroutine(presentationCoroutine);
        }
        AudioManager.instance.StopAllAudioExceptBackground();
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(SkipButtonClicked);
        
    }*/

    /*public void CheckAnswer(int shapeIndex)
    {
        if (!Tutorial)
        {
            AudioManager.instance.DecreaseBackgroundMusicVolume();
            CanSelectBox = false;
        }
        if (voiceInstructionRepeatCoroutine != null)
        {
            StopCoroutine(voiceInstructionRepeatCoroutine);
        }
        if (shapeIndex == correctShapeHolderIndex)
        {
            CorrectAnswer(shapeIndex);
        }
        else
        {
            WrongAnswer(shapeIndex);
        }
    }*/

    public void StartPresentation(bool firstTime)
    {
        if (presentationCoroutine != null)
        {
            StopCoroutine(presentationCoroutine);
        }
        if (firstTime)
        {
            // Presentation start on level start
            presentationCoroutine = StartCoroutine(MascotAppear());
        }
        else
        {
            // Presentation start on clicking hint button
            AudioManager.instance.StopAllAudioExceptBackground();
            audioReplay.enabled = false;
            audioReplay.gameObject.SetActive(false);
            CanSelectBox = false;
            hand.anchoredPosition = Vector2.zero;
            boxParent.gameObject.SetActive(false);
            if (voiceInstructionRepeatCoroutine != null)
            {
                StopCoroutine(voiceInstructionRepeatCoroutine);
            }
            if (!mascotOnScreen)
            {
                mascot.MascotReappear();
                mascotOnScreen = true;
            }
            Tutorial = true;

            // skip button default listener is level skip, so changing it to presentation skip
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(PresentationSkipButtonClicked);
            skipButton.gameObject.SetActive(true);
            
        }
    }

    public void NextShapes()
    {
        if (nextShapeCoroutine != null)
        {
            StopCoroutine(nextShapeCoroutine);
        }
        nextShapeCoroutine = StartCoroutine(NextShapesCoroutine(Tutorial));
    }

    private IEnumerator NextShapesCoroutine(bool tutorial)
    {
        int numOfTriangles = 0, numOfCircles = 0, numOfSquares = 0, numOfRectangles = 0;
        if(Tutorial)
        {
            numOfTriangles = 2;
            numOfCircles = 3;
            numOfSquares = 3;
            numOfRectangles = 2;
        }
        bool[] triangles = new bool[triangleSprites.Length];
        bool[] squares = new bool[squareSprites.Length];
        bool[] rectangles = new bool[rectangleSprites.Length];
        bool[] circles = new bool[circleSprites.Length];
        int firstRectanglePosition = -1;
        List<int> shapeColoursLeft = new List<int>();
        for (int i = 0; i < circleSprites.Length; i++)
        {
            triangles[i] = false;
            squares[i] = false;
            rectangles[i] = false;
            circles[i] = false;
            shapeColoursLeft.Add(i);
        }

        bool[] shapeIndexes = new bool[10];
        List<int> shapePlacesLeft = new List<int>();
        for (int i = 0; i< 10; i++)
        {
            shapePlacesLeft.Add(i);
            shapeIndexes[i] = false;
        }
        int randomShapeIndex;
        int randomShape;
        int randomShapeColourIndex;
        int randomPlaceIndex;
        List<int> shapeIndexesLeft = new List<int>();
        for(int i = 0; i < numOfTriangles; i++)
        {
            shapeIndexesLeft.Add(0);
        }
        for (int i = 0; i < numOfCircles; i++)
        {
            shapeIndexesLeft.Add(1);
        }
        for (int i = 0; i < numOfSquares; i++)
        {
            shapeIndexesLeft.Add(2);
        }
        for (int i = 0; i < numOfRectangles; i++)
        {
            shapeIndexesLeft.Add(3);
        }

        for (int i = 0; i < 10; i++)
        {
            randomShapeIndex = Random.Range(0, shapeIndexesLeft.Count);
            randomShape = shapeIndexesLeft[randomShapeIndex];
            shapeIndexesLeft.RemoveAt(randomShapeIndex);
            randomPlaceIndex = Random.Range(0, shapePlacesLeft.Count);
            randomShapeColourIndex = Random.Range(0, shapeColoursLeft.Count);
            if(randomShape == 0)
            {
                //triangle
                shapes[shapePlacesLeft[randomPlaceIndex]].sprite = triangleSprites[shapeColoursLeft[randomShapeColourIndex]];
            }
            else if(randomShape == 1)
            {
                //circle
                shapes[shapePlacesLeft[randomPlaceIndex]].sprite = circleSprites[shapeColoursLeft[randomShapeColourIndex]];
            }
            else if (randomShape == 2)
            {
                //square
                shapes[shapePlacesLeft[randomPlaceIndex]].sprite = squareSprites[shapeColoursLeft[randomShapeColourIndex]];
            }
            else if (randomShape == 3)
            {
                //rectangle
                if(firstRectanglePosition >= 0)
                {
                    if((firstRectanglePosition < 5 && shapePlacesLeft[randomPlaceIndex] >= 5) || (firstRectanglePosition >= 5 && shapePlacesLeft[randomPlaceIndex] < 5))
                    {
                        shapes[shapePlacesLeft[randomPlaceIndex]].sprite = rectangleSprites[shapeColoursLeft[randomShapeColourIndex]];
                    }
                    else
                    {
                        if(firstRectanglePosition < 5)
                        {
                            for(int j = 0; j < shapePlacesLeft.Count; j++)
                            {
                                if(shapePlacesLeft[j] >= 5)
                                {
                                    randomPlaceIndex = j;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < shapePlacesLeft.Count; j++)
                            {
                                if (shapePlacesLeft[j] < 5)
                                {
                                    randomPlaceIndex = j;
                                    break;
                                }
                            }
                        }

                        shapes[shapePlacesLeft[randomPlaceIndex]].sprite = rectangleSprites[shapeColoursLeft[randomShapeColourIndex]];
                    }
                }
                else
                {
                    firstRectanglePosition = shapePlacesLeft[randomPlaceIndex];
                    shapes[shapePlacesLeft[randomPlaceIndex]].sprite = rectangleSprites[shapeColoursLeft[randomShapeColourIndex]];
                }
            }
            shapePlacesLeft.RemoveAt(randomPlaceIndex);
            shapeColoursLeft.RemoveAt(randomShapeColourIndex);
        }


        if (!tutorial)
        {
            EnableSpeaking();
            yield return new WaitForSeconds(mascot.PlayClip(2));
            DisableSpeaking();
            firstAttempt = true;
            CanSelectBox = true;

            StartVoiceInstructionCoroutine();
        }
        AudioManager.instance.IncreaseBackgroundMusicVolume();
    }

    public void StartVoiceInstructionCoroutine()
    {
        if (voiceInstructionRepeatCoroutine != null)
        {
            StopCoroutine(voiceInstructionRepeatCoroutine);
        }
        voiceInstructionRepeatCoroutine = StartCoroutine(StartVoiceInstructionAfterTime(15f));
    }

    private IEnumerator StartVoiceInstructionAfterTime(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            CanSelectBox = false;
            yield return new WaitForSeconds(AudioManager.instance.Play("LV1_Mascot5"));
            CanSelectBox = true;
        }
    }

    private void DisableSpeaking()
    {
        if (mascotOnScreen)
        {
            mascot.speechDotsRect.gameObject.SetActive(false);
            mascot.speechBubble.gameObject.SetActive(false);
        }
    }

    private void EnableSpeaking()
    {
        if (mascotOnScreen)
        {
            mascot.speechBubble.gameObject.SetActive(true);
            mascot.speechDotsRect.gameObject.SetActive(true);
        }
    }

    /*private void CorrectAnswer(int shapeHolderIndex)
    {
        if (!Tutorial)
        {
            setNumber++;
            if (firstAttempt || (setNumber % 4 == 0 && setNumber > 0 && setNumber < 9))
            {
                Progress++;
            }
            if (firstAttempt && setNumber > 8)
            {
                Progress = progressSprites.Length - 1;
                correctAnswerStreak++;
            }
        }
       
        StartCoroutine(CorrectAnswerAnimation());
    }*/

    

    public void ToggleBackgroundMusic()
    {
        AudioMute = !AudioMute;
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

    public void HomeButtonClicked()
    {
        Time.timeScale = 0f;
        MenuButtonsOnScreen = true;
        AudioManager.instance.PauseAudio();
        CanSelectBox = false;
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
        CanSelectBox = true;
        homeButtonIcons.SetActive(false);
        backButtonGameObject.SetActive(false);
    }

    public void ReplayAudioInstructions()
    {
        StartCoroutine(AudioInstructionsRepeatCoroutine());
    }


    private IEnumerator AudioInstructionsRepeatCoroutine()
    {
        AudioManager.instance.DecreaseBackgroundMusicVolume();
        CanSelectBox = false;
        yield return new WaitForSeconds(mascot.PlayClip(1));
        CanSelectBox = true;
        AudioManager.instance.IncreaseBackgroundMusicVolume();
    }

    /*private void WrongAnswer(int shapeHolderIndex)
    {
        if (!Tutorial)
        {
            firstAttempt = false;
            correctAnswerStreak = 0;
        }
        

        StartCoroutine(WrongAnswerAnimation(shapeHolderIndex));
    }*/
    

    private void KillAllTweens()
    {
        hand.DOKill();
        handImage.DOKill();
        hand.transform.DOKill();
        mascot.KillAllTweens();
    }

    public void NextLevel()
    {
        StopAllCoroutines();
        KillAllTweens();
        Time.timeScale = 1f;
        SceneLoader.instance.LoadNextLevel(Constants.Level2.nextLevel);
        AudioManager.instance.Play("BackgroundMusic");
    }

    public void ReplayLevel()
    {
        levelCompletedIcons.SetActive(false);
        musicButton.image.sprite = buttonImage;
        AudioManager.instance.Stop("BackgroundMusic");
        AudioManager.instance.Play("BackgroundMusic");
        audioReplay.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(true);
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(SkipButtonClicked);
        hand.DOAnchorPos(new Vector2(0f, 0f), 0f);
        numberOfCircles = 0; numberOfTriangles = 0; numberOfRectangles = 0; numberOfSquares = 0; numberOfCircles2 = 0; numberOfTriangles2 = 0; numberOfRectangles2 = 0; numberOfSquares2 = 0;
        numberOfRectangles3 = 0; numberOfSquares3 = 0;
        confetti.SetActive(false);
        boxParent.SetActive(true);
        upperShapes[0].images.gameObject.SetActive(true);
        for( int i = 0; i < shape.Count; i++)
		{
            for(int j = 0; j < shape[i].image.Length; j++)
			{
                shape[0].image[j].gameObject.SetActive(true);
                shape[i].image[j].gameObject.transform.GetChild(0).gameObject.SetActive(false);
			}
		}
        StartCoroutine(StartOver());
    }

    IEnumerator StartOver()
	{
        yield return new WaitForSeconds(mascot.PlayClip(1));
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[i].image[j].gameObject.GetComponent<Button>().enabled = true;
            }
        }
    }

    /*private void OnDestroy()
    {
        HandManagerLV1.selectedShape -= CheckAnswer;
    }*/

    public void BackButtonClicked()
	{
        Time.timeScale = 0f;
        MenuButtonsOnScreen = true;
        AudioManager.instance.PauseAudio();
        CanSelectBox = false;
        AudioManager.instance.PlayMenuAudio("HomeButtonAudio");
        backButtonGameObject.SetActive(true);
    }
    public void PreviousLevelClicked()
    {
        AudioManager.instance.Stop("HomeButtonAudio");
        Time.timeScale = 0f;
        AudioManager.instance.Stop("BackgroundMusic");
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        Time.timeScale = 1f;
        AudioManager.instance.Play("BackgroundMusic");
        Level1Manager.instance.skipButton.gameObject.SetActive(true);
        Level1Manager.instance.skipButton.onClick.RemoveAllListeners();
        Level1Manager.instance.skipButton.onClick.AddListener(SkipButtonClicked);
        PlayerPrefs.GetInt("LevelBar");
        PlayerPrefs.GetString("Score");
        if (setNumber != 10)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            Time.timeScale = 1f;
        }
    }

    public void StartPresentation()
	{
        PlayerPrefs.SetInt("SetNumber", setNumber);
        hintButton.interactable = false;
        musicButton.image.sprite = buttonImage;
        AudioManager.instance.Stop("BackgroundMusic");
        AudioManager.instance.Play("BackgroundMusic");
        boxParent.SetActive(false);
        for(int i = 0; i < sets.Length; i++)
		{
            sets[i].gameObject.SetActive(false);
        }
        audioReplay.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(true);
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(SkipPresentationButtonClicked);
        mascot.rect.DOAnchorPos(new Vector2(0f, -90f), 0f);
        mascot.mascotImage.sprite = mascot.mascotNormalSprite;
        mascot.gameObject.SetActive(true);
        mascot.SpeechBubbleRight();
        hand.DOAnchorPos(new Vector2(0f, 0f), 0f);
        StartCoroutine(Presentation());

	}
    IEnumerator Presentation()
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
        mascot.rect.DOAnchorPos(new Vector3(344f, -90f), 2f);
        mascot.rect.DOScale(1f, 2f);

        yield return oneSec;
        yield return oneSec;
        boxParent.SetActive(true);
        shapeImage.gameObject.SetActive(true);
        mascot.SpeechBubbleLeft();
        onoff = true;
        mascot.ChangeMascotImage();
        EnableSpeaking();
        yield return new WaitForSeconds(mascot.PlayClip(1));
        mascot.MascotDisappear();
        yield return oneSec;
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].gameObject.SetActive(true);
        }
        handGameObject.SetActive(true);
        hand.DOAnchorPos(new Vector2(-264f, -112f), 2f);
        yield return oneSec;
        yield return oneSec;
        tickImage[0].gameObject.SetActive(true);
        AudioManager.instance.Play("CorrectAnswer");
        yield return oneSec;
        hand.DOAnchorPos(new Vector2(39f, -225f), 2f);
        yield return oneSec;
        yield return oneSec;
        tickImage[1].gameObject.SetActive(true);
        AudioManager.instance.Play("CorrectAnswer");
        yield return oneSec;
        hand.DOAnchorPos(new Vector2(203f, -225f), 2f);
        yield return oneSec;
        yield return oneSec;
        tickImage[2].gameObject.SetActive(true);
        AudioManager.instance.Play("CorrectAnswer");
        yield return oneSec;
        hand.DOAnchorPos(new Vector2(340f, -111f), 2f);
        yield return oneSec;
        yield return oneSec;
        tickImage[3].gameObject.SetActive(true);
        AudioManager.instance.Play("CorrectAnswer");
        yield return oneSec;
        yield return oneSec;
        for (int i = 0; i < tickImage.Length; i++)
        {
            tickImage[i].gameObject.SetActive(false);
        }
        shapes[1].gameObject.SetActive(false);
        shapes[2].gameObject.SetActive(false);
        shapes[3].gameObject.SetActive(false);
        shapes[5].gameObject.SetActive(false);
        shapes[6].gameObject.SetActive(false);
        shapes[9].gameObject.SetActive(false);
        handGameObject.SetActive(false);
        yield return oneSec;
        yield return oneSec;
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].gameObject.SetActive(false);
        }
        shapeImage.gameObject.SetActive(false);
        mascot.MascotReappear();
        EnableSpeaking();
        yield return new WaitForSeconds(mascot.PlayClip(2));
        hand.DOAnchorPos(new Vector2(0f, 0f), 2f);
        yield return oneSec;
        yield return oneSec;
        audioReplayImage.gameObject.SetActive(true);
        audioReplay.GetComponent<Button>().enabled = false;
        handGameObject.SetActive(true);
        hand.DOAnchorPos(new Vector2(394f, 103f), 2f);
        yield return oneSec;
        AudioManager.instance.Play("ReplayAudio");
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;
        handGameObject.SetActive(false);
        mascot.MascotDisappear();
        yield return oneSec;
        boxParent.SetActive(false);
        presentationEndIcons.SetActive(true);
        presentationEndIcons.transform.GetChild(0).GetComponent<Button>().enabled = false;
        presentationEndIcons.transform.GetChild(1).GetComponent<Button>().enabled = false;
        yield return new WaitForSeconds(AudioManager.instance.Play("PresentationCompletionInstructions"));
        presentationEndIcons.transform.GetChild(0).GetComponent<Button>().enabled = true;
        presentationEndIcons.transform.GetChild(1).GetComponent<Button>().enabled = true;
    }

    public void SkipPresentationButtonClicked()
    {
        Time.timeScale = 0f;
        AudioManager.instance.PauseAudio();
        AudioManager.instance.Play("PresentationSkip");
        skipButton.gameObject.SetActive(false);
        presentationSkipButtonIcons.SetActive(true);
    }

    IEnumerator PresentationSkipAnimation()
    {
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(SkipButtonClicked);
        audioReplay.interactable = true;
        if (Constants.Level4.canSkip)
        {
            skipButton.gameObject.SetActive(true);
        }
        else
        {
            skipButton.gameObject.SetActive(false);
        }
        for(int i = 0; i < shapes.Length; i++)
		{
            shapes[i].gameObject.SetActive(false);
		}
        hand.gameObject.SetActive(false);
        shapeImage.gameObject.SetActive(false);
        for(int i = 0; i < tickImage.Length; i++)
		{
            tickImage[i].gameObject.SetActive(false);
		}
        mascot.MascotDisappear();
        mascotOnScreen = false;
        Tutorial = false;
        yield return oneSec;
        audioReplay.gameObject.SetActive(true);
        audioReplay.enabled = true;
        CanSelectBox = false;
        boxParent.SetActive(true);
        backButton.interactable = true;
        hintButton.interactable = true;
        numberOfCircles = 0; numberOfTriangles = 0; numberOfRectangles = 0; numberOfSquares = 0; numberOfCircles2 = 0; numberOfTriangles2 = 0; numberOfRectangles2 = 0; numberOfSquares2 = 0;
        numberOfRectangles3 = 0; numberOfSquares3 = 0;
        Debug.Log("Number Of Circles clicked: " + numberOfCircles); Debug.Log("Number Of Triangles clicked: " + numberOfTriangles); Debug.Log("Number Of Rectangles clicked: " + numberOfRectangles);
        Debug.Log("Number Of Squares clicked: " + numberOfSquares); Debug.Log("Number Of Circles2 clicked: " + numberOfCircles2); Debug.Log("Number Of Triangles2 clicked: " + numberOfTriangles2);
        Debug.Log("Number Of Rectangles2 clicked: " + numberOfRectangles2); Debug.Log("Number Of Squares2 clicked: " + numberOfSquares2); Debug.Log("Number Of Rectangles3 clicked: " + numberOfRectangles3);
        Debug.Log("Number Of Squares3 clicked: " + numberOfSquares3);
        PlayerPrefs.GetInt("SetNumber");
        SetNumberData();
    }

    public void GoToHomeScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void Update()
	{
		if (onoff)
		{
            speechBubbleImage.gameObject.SetActive(true);
            speechDotsImage.gameObject.SetActive(true);
		}
		else
		{
            speechBubbleImage.gameObject.SetActive(false);
            speechDotsImage.gameObject.SetActive(false);
        }
	}

    IEnumerator WrongAnswerMusic()
	{
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[i].image[j].gameObject.GetComponent<Button>().enabled = false;
            }
        }
        yield return oneSec;
        yield return oneSec;
        AudioManager.instance.Play("TryAgain");
        for(int i = 0; i < shape.Count; i++)
		{
            for(int j = 0; j < shape[i].image.Length; j++)
			{
                shape[i].image[j].transform.GetChild(1).gameObject.SetActive(false);
			}
		}
        yield return oneSec;
        yield return oneSec;
        yield return new WaitForSeconds(mascot.PlayClip(1));
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[i].image[j].gameObject.GetComponent<Button>().enabled = true;
            }
        }
    }

    IEnumerator WaitForTwoSeconds1()
	{
       
        yield return oneSec;
        yield return oneSec;
        for (int i = 0; i < shape.Count; i++)
		{
            for(int j = 0; j < shape[i].image.Length; j++)
			{
                shape[0].image[j].gameObject.SetActive(false);
			}
		}
        upperShapes[0].images.gameObject.SetActive(false);
        yield return oneSec;
        sets[1].SetActive(true);
        upperShapes[1].images.gameObject.SetActive(true);
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[1].image[j].gameObject.SetActive(true);
            }
        }
    }

    IEnumerator WaitForTwoSeconds2()
    {
        yield return oneSec;
        yield return oneSec;
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[1].image[j].gameObject.SetActive(false);
            }
        }
        upperShapes[1].images.gameObject.SetActive(false);
        yield return oneSec;
        sets[2].SetActive(true);
        upperShapes[2].images.gameObject.SetActive(true);
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[2].image[j].gameObject.SetActive(true);
            }
        }
    }

    IEnumerator WaitForTwoSeconds3()
	{
        yield return oneSec;
        yield return oneSec;
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[2].image[j].gameObject.SetActive(false);
            }
        }
        upperShapes[2].images.gameObject.SetActive(false);
        yield return oneSec;
        sets[3].SetActive(true);
        upperShapes[3].images.gameObject.SetActive(true);
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[3].image[j].gameObject.SetActive(true);
            }
        }
    }

    IEnumerator WaitForTwoSeconds4()
	{
        yield return oneSec;
        yield return oneSec;
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[3].image[j].gameObject.SetActive(false);
            }
        }
        upperShapes[3].images.gameObject.SetActive(false);
        yield return oneSec;
        sets[4].SetActive(true);
        upperShapes[4].images.gameObject.SetActive(true);
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[4].image[j].gameObject.SetActive(true);
            }
        }
    }

    IEnumerator WaitForTwoSeconds5()
	{
        yield return oneSec;
        yield return oneSec;
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[4].image[j].gameObject.SetActive(false);
            }
        }
        upperShapes[4].images.gameObject.SetActive(false);
        yield return oneSec;
        sets[5].SetActive(true);
        upperShapes[5].images.gameObject.SetActive(true);
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[5].image[j].gameObject.SetActive(true);
            }
        }
    }

    IEnumerator WaitForTwoSeconds6()
	{
        yield return oneSec;
        yield return oneSec;
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[5].image[j].gameObject.SetActive(false);
            }
        }
        upperShapes[5].images.gameObject.SetActive(false);
        yield return oneSec;
        sets[6].SetActive(true);
        upperShapes[6].images.gameObject.SetActive(true);
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[6].image[j].gameObject.SetActive(true);
            }
        }
    }

    IEnumerator WaitForTwoSeconds7()
	{
        yield return oneSec;
        yield return oneSec;
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[6].image[j].gameObject.SetActive(false);
            }
        }
        upperShapes[6].images.gameObject.SetActive(false);
        yield return oneSec;
        sets[7].SetActive(true);
        upperShapes[7].images.gameObject.SetActive(true);
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[7].image[j].gameObject.SetActive(true);
            }
        }
    }

    IEnumerator WaitForTwoSeconds8()
	{
        yield return oneSec;
        yield return oneSec;
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[7].image[j].gameObject.SetActive(false);
            }
        }
        upperShapes[7].images.gameObject.SetActive(false);
        yield return oneSec;
        sets[8].SetActive(true);
        upperShapes[8].images.gameObject.SetActive(true);
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[8].image[j].gameObject.SetActive(true);
            }
        }
    }

    IEnumerator WaitForTwoSeconds9()
	{
        yield return oneSec;
        yield return oneSec;
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[8].image[j].gameObject.SetActive(false);
            }
        }
        upperShapes[8].images.gameObject.SetActive(false);
        yield return oneSec;
        sets[9].SetActive(true);
        upperShapes[9].images.gameObject.SetActive(true);
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[9].image[j].gameObject.SetActive(true);
            }
        }
    }

    IEnumerator WaitForTwoSeconds10()
	{
        yield return oneSec;
        yield return oneSec;
        for (int i = 0; i < shape.Count; i++)
        {
            for (int j = 0; j < shape[i].image.Length; j++)
            {
                shape[9].image[j].gameObject.SetActive(false);
            }
        }
        upperShapes[9].images.gameObject.SetActive(false);
        yield return oneSec;
        boxParent.SetActive(false);
        confetti.SetActive(true);
        Constants.rewards = int.Parse(rewardsText.text) + 10;
        rewardsText.text = Constants.rewards.ToString();
        PlayerPrefs.SetString("Score", rewardsText.text);
        PlayerPrefs.SetInt("LevelBar", Constants.Level1.progress);
        yield return new WaitForSeconds(AudioManager.instance.Play("LevelComplete") + 1f);
        EnableSpeaking();
        levelCompletedIcons.SetActive(true);
        yield return new WaitForSeconds(AudioManager.instance.Play("NextLevelInstructions") + 1f);
        DisableSpeaking();
        CanSelectBox = false;
        AudioManager.instance.Stop("BackgroundMusic");
    }

	public void PlayGame()
	{
        AudioManager.instance.Play("BackgroundMusic");
        skipButton.gameObject.SetActive(false);
        presentationEndIcons.SetActive(false);
        boxParent.SetActive(true);
        PlayerPrefs.GetInt("SetNumber");
        SetNumberData();
	} 
	
    public void SetNumberData()
	{
        if (PlayerPrefs.GetInt("SetNumber") == 0)
        {
            audioReplay.interactable = true;
            sets[0].SetActive(true);
            for (int i = 0; i < shape.Count; i++)
            {
                for (int j = 0; j < shape[i].image.Length; j++)
                {
                    shape[0].image[j].gameObject.SetActive(true);
                }
            }
            upperShapes[0].images.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetNumber") == 1)
        {
            audioReplay.interactable = true;
            sets[1].SetActive(true);
            for (int i = 0; i < shape.Count; i++)
            {
                for (int j = 0; j < shape[i].image.Length; j++)
                {
                    shape[1].image[j].gameObject.SetActive(true);
                }
            }
            upperShapes[1].images.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetNumber") == 2)
        {
            audioReplay.interactable = true;
            sets[2].SetActive(true);
            for (int i = 0; i < shape.Count; i++)
            {
                for (int j = 0; j < shape[i].image.Length; j++)
                {
                    shape[2].image[j].gameObject.SetActive(true);
                }
            }
            upperShapes[2].images.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetNumber") == 3)
        {
            audioReplay.interactable = true;
            sets[3].SetActive(true);
            for (int i = 0; i < shape.Count; i++)
            {
                for (int j = 0; j < shape[i].image.Length; j++)
                {
                    shape[3].image[j].gameObject.SetActive(true);
                }
            }
            upperShapes[3].images.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetNumber") == 4)
        {
            audioReplay.interactable = true;
            sets[4].SetActive(true);
            for (int i = 0; i < shape.Count; i++)
            {
                for (int j = 0; j < shape[i].image.Length; j++)
                {
                    shape[4].image[j].gameObject.SetActive(true);
                }
            }
            upperShapes[4].images.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetNumber") == 5)
        {
            audioReplay.interactable = true;
            sets[5].SetActive(true);
            for (int i = 0; i < shape.Count; i++)
            {
                for (int j = 0; j < shape[i].image.Length; j++)
                {
                    shape[5].image[j].gameObject.SetActive(true);
                }
            }
            upperShapes[5].images.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetNumber") == 6)
        {
            audioReplay.interactable = true;
            sets[6].SetActive(true);
            for (int i = 0; i < shape.Count; i++)
            {
                for (int j = 0; j < shape[i].image.Length; j++)
                {
                    shape[6].image[j].gameObject.SetActive(true);
                }
            }
            upperShapes[6].images.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetNumber") == 7)
        {
            audioReplay.interactable = true;
            sets[7].SetActive(true);
            for (int i = 0; i < shape.Count; i++)
            {
                for (int j = 0; j < shape[i].image.Length; j++)
                {
                    shape[7].image[j].gameObject.SetActive(true);
                }
            }
            upperShapes[7].images.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetNumber") == 8)
        {
            audioReplay.interactable = true;
            sets[8].SetActive(true);
            for (int i = 0; i < shape.Count; i++)
            {
                for (int j = 0; j < shape[i].image.Length; j++)
                {
                    shape[8].image[j].gameObject.SetActive(true);
                }
            }
            upperShapes[8].images.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("SetNumber") == 9)
        {
            audioReplay.interactable = true;
            sets[9].SetActive(true);
            for (int i = 0; i < shape.Count; i++)
            {
                for (int j = 0; j < shape[i].image.Length; j++)
                {
                    shape[9].image[j].gameObject.SetActive(true);
                }
            }
            upperShapes[9].images.gameObject.SetActive(true);
        }
    }

}
