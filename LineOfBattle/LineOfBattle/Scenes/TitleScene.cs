﻿using System;
using Reactive.Bindings.Extensions;
using SharpDX.Direct2D1;
using ShootighLibrary;
using ShootighLibrary.Device;

namespace LineOfBattle.Scenes
{
    internal class TitleScene : SceneBase
    {
        private bool _ignoreOnce = true;

        protected override void Initialize()
            => Mouse.Left.Subscribe( isPressed => {
                if ( isPressed ) {
                    if ( _ignoreOnce ) {
                        _ignoreOnce = false;
                    } else {
                        GameInstance.TransitScene<BattleScene>();
                    }
                }
            } ).AddTo( Disposables );

        protected override void Execute( RenderTarget target )
        {
        }
    }
}
