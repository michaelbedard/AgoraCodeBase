using System;
using System.Collections.Generic;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Utility.MVC;
using Agora.Core.Enums;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _src.Code.Game.Modules.Zone
{
    public partial class Zone
    {
        private const float MoveTransitionTime = 1f;
        
        public Transform AddCard(ICard card)
        {
            // add card
            Cards.Add(card);
            card.ParentZone = this;
            card.CanBeDrag = false;
            
            // get target transform
            var newCard = new GameObject("CardTransform");
            var tr = newCard.transform;

            tr.rotation = Transform.rotation;
            tr.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            
            var basePosition = Transform.position + new Vector3(0, (Cards.Count + 1) * 0.02f, 0);
            
            // Add randomness to the position
            float randomXOffset = Random.Range(-0.1f, 0.1f); // Change range as needed
            float randomZOffset = Random.Range(-0.1f, 0.1f); // Change range as needed
            basePosition += new Vector3(randomXOffset, 0, randomZOffset);

            // Set the position
            tr.position = basePosition;

            // Add randomness to the rotation
            float randomZRotation = Random.Range(-10f, 10f); // Change range as needed
            tr.Rotate(90, 0, randomZRotation);

            if (StackingMethod == ZoneStackingMethod.Diagonal)
            {
                tr.position += new Vector3(0.3f, 0, -0.45f) * (Cards.Count - 1);
            }

            return tr;
        }

        public void RemoveCard(ICard card)
        {
            Cards.Remove(card);
            card.ParentZone = null;
        }

        public Transform AddMarker(IMarker marker)
        {
            // add marker
            Markers.Add(marker);
            marker.ParentZone = this;
            
            // return target transform
            var markersTransforms = GetMarkerTransforms();
            return markersTransforms[Markers.Count - 1];
        }

        public void RemoveMarker(IMarker marker)
        {
            Markers.Remove(marker);
            marker.ParentZone = null;
        }

        public Transform AddToken(IToken token)
        {
            // add marker
            Tokens.Add(token);
            token.ParentZone = this;
            
            // return target transform
            var tokensTransforms = GetTokenTransforms();
            return tokensTransforms[Tokens.Count - 1];
        }

        public void RemoveToken(IToken token)
        {
            Tokens.Remove(token);
            token.ParentZone = null;
        }

        public Sequence UpdateObjectPositions(IBaseController objectToIgnore = null, bool instantaneous = true)
        {
            var s = DOTween.Sequence();
            
            var markersTransforms = GetMarkerTransforms();
            foreach (var targetTransform in markersTransforms)
            {
                var marker = Markers[markersTransforms.IndexOf(targetTransform)];
                
                if (marker == objectToIgnore)
                    continue;

                if (instantaneous)
                {
                    marker.Transform.position = targetTransform.position;
                    marker.Transform.rotation = targetTransform.rotation;
                    marker.Transform.localScale = targetTransform.localScale;
                }
                else
                {
                    s.Join(marker.Transform.DOMove(targetTransform.position, MoveTransitionTime));
                    s.Join(marker.Transform.DORotateQuaternion(targetTransform.rotation, MoveTransitionTime));
                    s.Join(marker.Transform.DOScale(targetTransform.localScale, MoveTransitionTime));
                }
            }
            
            var tokensTransforms = GetTokenTransforms();
            foreach (var targetTransform in tokensTransforms)
            {
                var token = Tokens[tokensTransforms.IndexOf(targetTransform)];
                
                if (token == objectToIgnore)
                    continue;

                if (instantaneous)
                {
                    token.Transform.position = targetTransform.position;
                    token.Transform.rotation = targetTransform.rotation;
                    token.Transform.localScale = targetTransform.localScale;
                }
                else
                {
                    s.Join(token.Transform.DOMove(targetTransform.position, MoveTransitionTime));
                    s.Join(token.Transform.DORotateQuaternion(targetTransform.rotation, MoveTransitionTime));
                    s.Join(token.Transform.DOScale(targetTransform.localScale, MoveTransitionTime));
                }
            }

            return DOTween.Sequence();
        }

        /// <summary>
        /// Private
        /// </summary>
        
        private List<Transform> GetMarkerTransforms()
        {
            var markersTransforms = new List<Transform>();
            int count = Markers.Count;

            for (int i = 0; i < count; i++)
            {
                // Create a temporary GameObject
                var temp = new GameObject("temp");
                var tr = temp.transform;

                // Set rotation
                tr.rotation = Transform.rotation;

                // Calculate position based on the number of markers
                if (count == 1)
                {
                    // Single marker at the center
                    tr.position = Transform.position + new Vector3(0, 0.4f, 0);
                }
                else if (count == 2)
                {
                    // Two markers side by side
                    float offset = 0.5f; // Distance between markers
                    tr.position = Transform.position + new Vector3((i == 0 ? -offset : offset), 0.4f, 0);
                }
                else if (count == 3)
                {
                    // Three markers in a triangle
                    float radius = 0.5f; // Distance from center to each marker
                    float angle = i * Mathf.PI * 2f / 3f; // 120-degree separation
                    tr.position = Transform.position + new Vector3(Mathf.Cos(angle) * radius, 0.4f, Mathf.Sin(angle) * radius);
                }
                else if (count == 4)
                {
                    // Four markers in a square
                    float offset = 0.5f; // Distance between markers
                    tr.position = Transform.position + new Vector3(
                        (i % 2 == 0 ? -offset : offset), 
                        0.4f, 
                        (i / 2 == 0 ? -offset : offset)
                    );
                }

                // Add the transform to the list and destroy the GameObject
                markersTransforms.Add(tr);
                GameObject.Destroy(temp);
            }

            return markersTransforms;
        }

        private List<Transform> GetTokenTransforms()
        {
            var tokensTransforms = new List<Transform>();
            int count = Tokens.Count;
            int maxPileSize = 3; // Max tokens per pile
            float pileOffset = 0.8f; // Distance between piles
            float tokenHeightOffset = 0.1f; // Vertical stacking offset

            int numberOfTokenPile = count <= maxPileSize ? 1 : count <= maxPileSize*2 ? 2 : count <= maxPileSize*3 ? 3 : 4;

            for (int i = 0; i < count; i++)
            {
                var temp = new GameObject("temp");
                var tr = temp.transform;

                // Set rotation
                tr.rotation = Transform.rotation;
                
                // Determine which pile this token belongs to
                int pileIndex = i / maxPileSize;
                int positionInPile = i % maxPileSize;

                // Base position for the pile
                Vector3 pilePosition;
                if (numberOfTokenPile == 1)
                {
                    // Single at the center
                    pilePosition = Transform.position + new Vector3(0, 0.0f, 0);
                } 
                else if (numberOfTokenPile == 2)
                {
                    // Two side by side
                    pilePosition = Transform.position + new Vector3((pileIndex == 0 ? -pileOffset : pileOffset), 0, 0);
                }
                else if (numberOfTokenPile == 3)
                {
                    // Three in a triangle
                    float angle = pileIndex * Mathf.PI * 2f / 3f; // 120-degree separation
                    pilePosition = Transform.position + new Vector3(Mathf.Cos(angle) * pileOffset, 0f, Mathf.Sin(angle) * pileOffset);
                }
                else if (numberOfTokenPile == 4)
                {
                    // Four in a square
                    pilePosition = Transform.position + new Vector3(
                        (pileIndex % 2 == 0 ? -pileOffset : pileOffset), 
                        0f, 
                        (pileIndex / 2 == 0 ? -pileOffset : pileOffset)
                    );
                }
                else
                {
                    throw new NotImplementedException();
                }

                // Stack tokens vertically within the pile
                tr.position = pilePosition + new Vector3(0, (positionInPile + 1) * tokenHeightOffset, 0);
                
                // Add randomness to the position
                float randomXOffset = Random.Range(-0.1f, 0.1f); // Change range as needed
                float randomZOffset = Random.Range(-0.1f, 0.1f); // Change range as needed
                tr.position += new Vector3(randomXOffset, 0, randomZOffset);

                // Add transform to list and destroy temp GameObject
                tokensTransforms.Add(tr);
                GameObject.Destroy(temp);
            }

            return tokensTransforms;
        }

    }
}