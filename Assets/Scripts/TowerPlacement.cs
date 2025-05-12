using System;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    // Private
    private GameObject towerPrefab;  // Префаб башни (должен быть назначен в инспекторе)
    private LayerMask placementLayer =  1 << 6; // Слой, на котором можно ставить башни
    private Camera mainCamera;
    private bool isBuildingMode=false;
    private bool isUpgradingMode=false;
    private bool isDeletingMode=false;
    [SerializeField]private GameObject building;
    [SerializeField]private GameObject upgrade;
    [SerializeField]private GameObject delete;


    void Start()
    {
        mainCamera = Camera.main; // Кэшируем камеру для оптимизации
        towerPrefab = (GameObject)Resources.Load("Tower");
    }

    void Update()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, placementLayer);
        int x,y;
        x =(int) Math.Round(mousePos.x);
        y =(int) Math.Round(mousePos.y);
        Vector2 pos = new Vector2((float)x<mousePos.x ? (float)x+0.5f:(float)x-0.5f, 
                                  (float)y<mousePos.y ? (float)y+0.5f:(float)y-0.5f);

        // Building
        if (isBuildingMode){
            if (hit.collider != null && Input.GetMouseButtonDown(0)) // Если попали в коллайдер : ЛКМ
            {
                int cost = 50;
                if (GameManager.Instance.GetMoney() >= cost && hit.collider.transform.tag!="Tower"){
                    Instantiate(towerPrefab, pos, Quaternion.identity);
                    GameManager.Instance.AddMoney(-cost); // Списание денег
                }
            }
            building.transform.position = pos;
        }

        // Upgrading
        if(isUpgradingMode){
            try{
                if(hit.collider != null && Input.GetMouseButtonDown(0)){ // Если попали в коллайдер : ЛКМ
                    int cost = hit.collider.gameObject.GetComponent<TowerScript>().Cost();
                    Debug.Log(cost);
                    if (GameManager.Instance.GetMoney() >= cost && hit.collider.transform.tag=="Tower"){
                        hit.collider.GetComponent<TowerScript>().Upgrade();
                        if(hit.collider.GetComponent<TowerScript>().Upgrade())
                            GameManager.Instance.AddMoney(-cost); // Списание денег
                    }
                }
            }catch{}
            upgrade.transform.position = pos;
        }

        // Deleting
        if(isDeletingMode){
            if(hit.collider != null && Input.GetMouseButtonDown(0)){ // Если попали в коллайдер : ЛКМ

                if (hit.collider.transform.tag=="Tower"){
                    GameManager.Instance.AddMoney((int)hit.collider.gameObject.GetComponent<TowerScript>().Cost()/2); // Добавление денег
                    Destroy(hit.collider.gameObject);
                }
            }
            delete.transform.position = pos;
        }
        
        
        if(Input.GetKeyDown(KeyCode.E)) {
            // Building
            isBuildingMode=!isBuildingMode;
            building.SetActive(isBuildingMode);
            // Upgrading
            isUpgradingMode=false;
            upgrade.SetActive(false);
            // Deleting
            isDeletingMode=false;
            delete.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Q)){
            // Upgrading
            isUpgradingMode=!isUpgradingMode;
            upgrade.SetActive(isUpgradingMode);
            // Building
            isBuildingMode=false;
            building.SetActive(false);
            // Deleting
            isDeletingMode=false;
            delete.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.D)){
            // Deleting
            isDeletingMode=!isDeletingMode;
            delete.SetActive(isDeletingMode);
            // Building
            isBuildingMode=false;
            building.SetActive(false);
            // Upgrading
            isUpgradingMode=false;
            upgrade.SetActive(false);
        }
    }
}