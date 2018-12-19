﻿using System;
using Reactive.Bindings.Extensions;
using SharpDX.Direct2D1;
using ShootighLibrary;
using ShootighLibrary.Device;

namespace LineOfBattle.Scenes
{
    internal class TitleScene : SceneBase
    {
        protected override void Initialize()
            => Mouse.Left.Subscribe( isPressed => {
                if ( isPressed ) {
                    GameInstance.TransitScene<BattleScene>();
                }
            } ).AddTo( Disposables );

        protected override void Execute( RenderTarget target )
        {
        }
    }
}
