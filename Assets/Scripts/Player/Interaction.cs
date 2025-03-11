using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; //0.05�ʸ��� üũ 
    private float lastCheckTime; //������ üũ �ð� 
    public float maxCheckDistance; //üũ�� �ִ� �Ÿ�
    public LayerMask layerMask; //��� ���̾ �޷��ִ� ���ӿ�����Ʈ�� �������� ����

    public GameObject curInteractGameObject; //���� ��ȣ�ۿ� ������ ���ӿ�����Ʈ 
    private IInteractable curInteractable; //���� ��ȣ�ۿ� ������ ���ӿ�����Ʈ�� IInteractable �������̽�

    public TextMeshProUGUI promptText; //��ȣ�ۿ� ������Ʈ �ؽ�Ʈ
    private Camera camera; //ī�޶� ����

    void Start()
    {
        camera = Camera.main; //���� ī�޶� ���� ��������
    }

    // Update is called once per frame
    void Update()
    {
        //���� �ð����� ��ȣ�ۿ� ������ ���ӿ�����Ʈ üũ
        if (Time.time - lastCheckTime > checkRate)
        {
            //������ üũ �ð� ����
            lastCheckTime = Time.time;

            //ī�޶� �������� ���� �߻�(���ݾ� ���� �߽ɿ��� �߻��ϰ� ����)
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit; //����ĳ��Ʈ�� ���� ������ ������ ����

            //����ĳ��Ʈ�� ���� ��ȣ�ۿ� ������ ���ӿ�����Ʈ�� ã��
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                //���� ��ȣ�ۿ� ������ ���ӿ�����Ʈ�� �ٲ���� ��
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    //���� ��ȣ�ۿ� ������ ���ӿ�����Ʈ ���� ����
                    curInteractGameObject = hit.collider.gameObject;
                    //���� ��ȣ�ۿ� ������ ���ӿ�����Ʈ�� IInteractable �������̽� ��������
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    //������Ʈ ����� �ؽ�Ʈ ����
                    SetPromptText();
                }
            }
            //��ȣ�ۿ� ������ ���ӿ�����Ʈ�� ���� ��
            else
            {
                //���� ��ȣ�ۿ� ������ ���ӿ�����Ʈ ���� �ʱ�ȭ
                curInteractGameObject = null;
                //���� ��ȣ�ۿ� ������ ���ӿ�����Ʈ�� IInteractable �������̽� �ʱ�ȭ
                curInteractable = null;
                //������Ʈ �ؽ�Ʈ ��Ȱ��ȭ
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        //������Ʈ �ؽ�Ʈ Ȱ��ȭ
        promptText.gameObject.SetActive(true);
        //������Ʈ �ؽ�Ʈ ����
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        //��ȣ�ۿ� ��ư�� ������ ��
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            //��ȣ�ۿ� ������ ���ӿ�����Ʈ�� OnInteract �Լ� ȣ��
            curInteractable.OnInteract();
            //���� ��ȣ�ۿ� ������ ���ӿ�����Ʈ ���� �ʱ�ȭ
            curInteractGameObject = null;
            //���� ��ȣ�ۿ� ������ ���ӿ�����Ʈ�� IInteractable �������̽� �ʱ�ȭ
            curInteractable = null;
            //������Ʈ �ؽ�Ʈ ��Ȱ��ȭ
            promptText.gameObject.SetActive(false);
        }
    }
}