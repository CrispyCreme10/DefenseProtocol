using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HudController : MonoBehaviour {
    [SerializeField] private MapInfo mapInfo;
    [SerializeField] private PlayerInfo playerInfo;
    
    private VisualElement _root;
    private VisualElement _bottomPanel;
    private VisualElement _dragElement;
    
    private void Awake() {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _bottomPanel = _root.Q("BottomPanel");
        
        // convert tower points to screen points

        _root.RegisterCallback<MouseMoveEvent>(evt => {
            if (_dragElement == null) return;
            
            var mousePos = Input.mousePosition;
            var mousePosAdj = new Vector2(mousePos.x, Screen.height - mousePos.y);
            var mouseAdj = RuntimePanelUtils.ScreenToPanel(_root.panel, mousePosAdj);

            _dragElement.style.visibility = Visibility.Visible;
            _dragElement.style.top = mouseAdj.y - _dragElement.worldBound.height / 2;
            _dragElement.style.left = mouseAdj.x - _dragElement.worldBound.width / 2;
            
            // check if mouse is in tower screen point bounds
            // IF SO, snap the drag visual element to the that screen point and change to green
            // ELSE, keep to cursor and drag element color is red
        });
        
        _root.RegisterCallback<MouseUpEvent>(evt => {
            if (_dragElement == null) return;

            _root.Remove(_dragElement);
            _dragElement = null;
        });
        
        BuildBottomPanel();
    }

    private void BuildBottomPanel() {
        foreach (var stackTower in playerInfo.Towers) {
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
            
            container.RegisterCallback<PointerDownEvent>(evt => {
                _dragElement = new VisualElement {
                    name = "DragElement",
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
}
