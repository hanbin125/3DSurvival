using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; //0.05초마다 체크 
    private float lastCheckTime; //마지막 체크 시간 
    public float maxCheckDistance; //체크할 최대 거리
    public LayerMask layerMask; //어떠한 레이어가 달려있는 게임오브젝트를 추출할지 결정

    public GameObject curInteractGameObject; //현재 상호작용 가능한 게임오브젝트 
    private IInteractable curInteractable; //현재 상호작용 가능한 게임오브젝트의 IInteractable 인터페이스

    public TextMeshProUGUI promptText; //상호작용 프롬프트 텍스트
    private Camera camera; //카메라 정보

    void Start()
    {
        camera = Camera.main; //메인 카메라 정보 가져오기
    }

    // Update is called once per frame
    void Update()
    {
        //일정 시간마다 상호작용 가능한 게임오브젝트 체크
        if (Time.time - lastCheckTime > checkRate)
        {
            //마지막 체크 시간 갱신
            lastCheckTime = Time.time;

            //카메라 기준으로 레이 발사(절반씩 나눠 중심에서 발사하게 만듬)
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit; //레이캐스트에 맞은 정보를 저장할 변수

            //레이캐스트를 통해 상호작용 가능한 게임오브젝트를 찾음
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                //현재 상호작용 가능한 게임오브젝트가 바뀌었을 때
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    //현재 상호작용 가능한 게임오브젝트 정보 갱신
                    curInteractGameObject = hit.collider.gameObject;
                    //현재 상호작용 가능한 게임오브젝트의 IInteractable 인터페이스 가져오기
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    //프롬프트 출력할 텍스트 설정
                    SetPromptText();
                }
            }
            //상호작용 가능한 게임오브젝트가 없을 때
            else
            {
                //현재 상호작용 가능한 게임오브젝트 정보 초기화
                curInteractGameObject = null;
                //현재 상호작용 가능한 게임오브젝트의 IInteractable 인터페이스 초기화
                curInteractable = null;
                //프롬프트 텍스트 비활성화
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        //프롬프트 텍스트 활성화
        promptText.gameObject.SetActive(true);
        //프롬프트 텍스트 설정
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        //상호작용 버튼을 눌렀을 때
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            //상호작용 가능한 게임오브젝트의 OnInteract 함수 호출
            curInteractable.OnInteract();
            //현재 상호작용 가능한 게임오브젝트 정보 초기화
            curInteractGameObject = null;
            //현재 상호작용 가능한 게임오브젝트의 IInteractable 인터페이스 초기화
            curInteractable = null;
            //프롬프트 텍스트 비활성화
            promptText.gameObject.SetActive(false);
        }
    }
}