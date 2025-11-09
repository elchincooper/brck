/******************************************************************************
 * Spine Runtimes License Agreement
 * Last updated January 1, 2020. Replaces all prior versions.
 *
 * Copyright (c) 2013-2020, Esoteric Software LLC
 *
 * Integration of the Spine Runtimes into software or otherwise creating
 * derivative works of the Spine Runtimes is permitted under the terms and
 * conditions of Section 2 of the Spine Editor License Agreement:
 * http://esotericsoftware.com/spine-editor-license
 *
 * Otherwise, it is permitted to integrate the Spine Runtimes into software
 * or otherwise create derivative works of the Spine Runtimes (collectively,
 * "Products"), provided that each user of the Products must obtain their own
 * Spine Editor license and redistribution of the Products in any form must
 * include this license and copyright notice.
 *
 * THE SPINE RUNTIMES ARE PROVIDED BY ESOTERIC SOFTWARE LLC "AS IS" AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL ESOTERIC SOFTWARE LLC BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES,
 * BUSINESS INTERRUPTION, OR LOSS OF USE, DATA, OR PROFITS) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THE SPINE RUNTIMES, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // << NEW: Required for the Event System

namespace Spine.Unity.Examples {
    
    // << FIX: Implement the IDragHandler interface
    public class DraggableTransform : MonoBehaviour, IDragHandler { 

        Camera mainCamera;

        void Start () {
            mainCamera = Camera.main;
        }
        
        // OLD CODE (Update and OnMouseDrag) HAS BEEN DELETED.
        // It is replaced by the OnDrag method, which only runs when a finger is actively dragging.

        // << FIX: Use the mobile-optimized OnDrag method
        public void OnDrag (PointerEventData eventData) {
            
            // 1. Convert the screen position of the current touch/pointer to world space.
            Vector3 mouseCurrentWorld = mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, -mainCamera.transform.position.z));
            
            // 2. The eventData contains the delta from the *previous* frame, 
            // but for movement we need the world-space delta, not the screen-space delta. 
            // The cleanest way is to use the existing logic (transform.Translate) 
            // but with a modified world-space delta.
            
            // Calculate the position difference in world space (since it was calculated in Update before)
            // Since we don't have mousePreviousWorld in this context, we'll use a simpler
            // drag translation based on the screen delta converted to world movement.

            // The original logic required the mousePreviousWorld to be calculated every frame.
            // For a direct drag fix, we use the screen delta (eventData.delta) and adjust 
            // it for the world-space scale.
            
            Vector3 worldDelta = mainCamera.ScreenToWorldPoint(eventData.delta) - mainCamera.ScreenToWorldPoint(Vector2.zero);

            // Apply the translation
            transform.Translate(worldDelta);
        }
    }
}