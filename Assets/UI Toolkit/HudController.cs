using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HudController : MonoBehaviour {
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private MapInfo mapInfo;
    [SerializeField] private PlayerInfo playerInfo;
    
    private VisualElement _root;
    private VisualElement _bottomPanel;
    private VisualElement _dragElement;
    private List<VisualElement> _towerScreenElements;
    private bool _isDragElementSnapped;
    private int _chosenTowerIndex = -1;
    private int _chosenPointIndex = -1;
    
    private void Awake() {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _bottomPanel = _root.Q("BottomPanel");
        
        // convert tower points to screen points
        const int spotSize = 100;
        _towerScreenElements = new List<VisualElement>();
        for (var index = 0; index < mapInfo.TurretPoints.Length; index++) {
            var turretPoint = mapInfo.TurretPoints[index];
            var turretPointScreenAdj = sceneCamera.WorldToScreenPoint(turretPoint);
            var top = Screen.height - turretPointScreenAdj.y - spotSize / 2f;
            var left = turretPointScreenAdj.x - spotSize / 2f;
            var towerSpotElement = new VisualElement {
                name = $"TowerSpotElement{index}",
                style = {
                    position = Position.Absolute,
                    top = top,
                    left = left,
                    height = spotSize,
                    width = spotSize,
                    // backgroundColor = new Color(0, 0, 0, 0.75f)
                }
            };
            
            var i = index;
            towerSpotElement.RegisterCallback<MouseEnterEvent>(evt => {
                if (_dragElement == null) return;

                _chosenPointIndex = i;
                
                // snap drag element to position
                _dragElement.style.top = top;
                _dragElement.style.left = left;
                _isDragElementSnapped = true;
                
                DragElementGreen();
            });
            
            towerSpotElement.RegisterCallback<MouseLeaveEvent>(evt => {
                if (_dragElement == null) return;

                _chosenPointIndex = -1;
                _isDragElementSnapped = false;
                DragElementRed();
            });
            
            _root.Add(towerSpotElement);
            _towerScreenElements.Add(towerSpotElement);
        }

        _root.RegisterCallback<MouseMoveEvent>(evt => {
            if (_dragElement == null) return;

            if (!_isDragElementSnapped) {
                var mousePos = Input.mousePosition;
                var mousePosAdj = new Vector2(mousePos.x, Screen.height - mousePos.y);
                var mouseAdj = RuntimePanelUtils.ScreenToPanel(_root.panel, mousePosAdj);

                _dragElement.style.visibility = Visibility.Visible;
                _dragElement.style.top = mouseAdj.y - _dragElement.worldBound.height / 2;
                _dragElement.style.left = mouseAdj.x - _dragElement.worldBound.width / 2;
            }
        });
        
        _root.RegisterCallback<MouseUpEvent>(evt => {
            if (_dragElement == null) return;

            _root.Remove(_dragElement);
            _dragElement = null;
            _isDragElementSnapped = false;
            
            // spawn tower if applicable
            if (_chosenTowerIndex >= 0 && _chosenPointIndex >= 0) {
                playerInfo.SpawnTower(_chosenTowerIndex, mapInfo.TurretPoints[_chosenPointIndex]);
            }
        });
        
        BuildBottomPanel();
    }

    private void OnEnable() {
        PlayerInfo.OnTowersChange += UpdateTowers;
    }

    private void OnDisable() {
        PlayerInfo.OnTowersChange -= UpdateTowers;
    }

    private void BuildBottomPanel() {
        for (var index = 0; index < playerInfo.Towers.Count; index++) {
            var stackTower = playerInfo.Towers[index];
            var container = new VisualElement {
                name = "TowerSlot",
                style = {
                    justifyContent = Justify.Center,
                    alignItems = Align.Center,
                    width = 100,
                    height = 100,
                    backgroundColor = new Color(255, 255, 255, 0.05f)
                }
            };

            var image = new Image {
                name = "TowerImage",
                sprite = stackTower.TowerSprite,
                style = {
                    width = 75,
                    height = 75
                }
            };

            var label = new Label {
                name = "CountLabel",
                text = stackTower.Amount.ToString(),
                style = {
                    position = Position.Absolute,
                    top = 0,
                    right = 0,
                    color = Color.white,
                    fontSize = 20f
                }
            };

            container.Add(label);
            container.Add(image);
            _bottomPanel.Add(container);

            var i = index;
            container.RegisterCallback<PointerDownEvent>(evt => {
                _chosenTowerIndex = i;

                _dragElement = new VisualElement {
                    name = "DragElement",
                    pickingMode = PickingMode.Ignore,
                    style = {
                        position = Position.Absolute,
                        visibility = Visibility.Hidden,
                        justifyContent = Justify.Center,
                        alignItems = Align.Center,
                        width = 100,
                        height = 100,
                        backgroundColor = new Color(255, 0, 0, 0.2f),
                        borderTopColor = new Color(255, 0, 0),
                        borderRightColor = new Color(255, 0, 0),
                        borderBottomColor = new Color(255, 0, 0),
                        borderLeftColor = new Color(255, 0, 0),
                        borderTopWidth = 2,
                        borderRightWidth = 2,
                        borderBottomWidth = 2,
                        borderLeftWidth = 2,
                    }
                };

                var dragImage = new Image {
                    name = "DragImage",
                    pickingMode = PickingMode.Ignore,
                    sprite = stackTower.TowerSprite,
                    style = {
                        width = 75,
                        height = 75,
                        opacity = 0.75f
                    }
                };

                _dragElement.Add(dragImage);
                _root.Add(_dragElement);
            });
        }
    }

    private void DragElementGreen() {
        _dragElement.style.backgroundColor = new Color(0, 255, 0, 0.2f);
        _dragElement.style.borderTopColor = new Color(0, 255, 0);
        _dragElement.style.borderRightColor = new Color(0, 255, 0);
        _dragElement.style.borderBottomColor = new Color(0, 255, 0);
        _dragElement.style.borderLeftColor = new Color(0, 255, 0);
    }
    
    private void DragElementRed() {
        _dragElement.style.backgroundColor = new Color(255, 0, 0, 0.2f);
        _dragElement.style.borderTopColor = new Color(255, 0, 0);
        _dragElement.style.borderRightColor = new Color(255, 0, 0);
        _dragElement.style.borderBottomColor = new Color(255, 0, 0);
        _dragElement.style.borderLeftColor = new Color(255, 0, 0);
    }

    private void UpdateTowers() {
        _bottomPanel.Clear();
        BuildBottomPanel();
    }
}
