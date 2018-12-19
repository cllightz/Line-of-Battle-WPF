﻿using SharpDX.Direct2D1;
using ShootighLibrary.Messenger;
using System;
using System.Diagnostics;

namespace ShootighLibrary
{
    /// <summary>
    /// ゲームのロジックを記述するクラスの抽象クラス。
    /// 実際のコードではこのクラスを実装する。
    /// </summary>
    public abstract class Game
    {
        #region Fields;
        /// <summary>
        /// ゲームのロジックを保持しているコントロール。
        /// 抽象クラス Game を実装するクラスのインスタンスの親インスタンスにあたる。
        /// </summary>
        public GameControl Control;

        protected RenderTarget Target;

        private bool _isSceneInitialized;
        #endregion

        #region Constuctor
        /// <summary>
        /// ゲームのロジックを保持している GameControl のインスタンスを渡す。
        /// </summary>
        /// <param name="control"></param>
        public Game( GameControl control )
        {
            Control = control;
            Mediator.Singleton.RegisterPublisher<string>( typeof( Game ) );
        }
        #endregion

        #region Properties
        public SceneBase CurrentScene { get; private set; }

        /// <summary>
        /// 描画領域の横幅。
        /// </summary>
        public float Width
            => (float)Control.ActualWidth;

        /// <summary>
        /// 描画領域の縦幅。
        /// </summary>
        public float Height
            => (float)Control.ActualHeight;
        #endregion

        #region Abstract Methods
        /// <summary>
        /// 初期化処理の抽象メソッド。
        /// </summary>
        public virtual void Initialize()
        {
            Mediator.Singleton.RegisterPublisher<Type>( typeof( Game ) );
            CurrentScene?.Initialize( this );
            _isSceneInitialized = false;
        }

        /// <summary>
        /// 毎フレームの処理の抽象メソッド。
        /// </summary>
        /// <param name="target">GameControl.Render( RenderTarget ) で受け取った RenderTarget のインスタンスを渡す。</param>
        public void MainLoop( RenderTarget target )
        {
            if ( !_isSceneInitialized ) {
                CurrentScene.Initialize( this );
                _isSceneInitialized = true;
            }

            CurrentScene.Execute( this, target );
        }

        public void TransitScene<TNewScene>() where TNewScene : SceneBase, new()
        {
            var oldScene = CurrentScene;

            try {
                CurrentScene = new TNewScene();
                oldScene?.Dispose();
                Mediator.Singleton.Publish( typeof( Game ), typeof( TNewScene ).Name );
                _isSceneInitialized = false;
                // CurrentScene?.Initialize( this );
                Debug.WriteLine( $"Scene Transition: from {oldScene?.GetType().FullName ?? "null"} to {typeof( TNewScene ).FullName}" );
            } catch ( Exception e ) {
                Debug.WriteLine( $"Exception Occurred in {nameof( Game )}.{nameof( TransitScene )}<{typeof( TNewScene ).FullName}>();\ne: {e}" );
                throw;
            }
        }
        #endregion
    }
}