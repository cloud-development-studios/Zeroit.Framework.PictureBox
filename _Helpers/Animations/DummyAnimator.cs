// ***********************************************************************
// Assembly         : Zeroit.Framework.PictureBox
// Author           : ZEROIT
// Created          : 12-25-2018
//
// Last Modified By : ZEROIT
// Last Modified On : 12-25-2018
// ***********************************************************************
// <copyright file="DummyAnimator.cs" company="Zeroit Dev Technologies">
//     Copyright © Zeroit Dev Technologies  2017. All Rights Reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel;

namespace Zeroit.Framework.PictureBox.Helpers.Animations
{
    /// <summary>
    /// Class DummyAnimator.
    /// </summary>
    /// <seealso cref="Zeroit.Framework.PictureBox.Helpers.Animations.AnimatorBase" />
    public class DummyAnimator : AnimatorBase
  {
        /// <summary>
        /// Initializes a new instance of the <see cref="DummyAnimator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public DummyAnimator(IContainer container)
      : base(container)
    {
    }

        /// <summary>
        /// Initializes a new instance of the <see cref="DummyAnimator"/> class.
        /// </summary>
        public DummyAnimator()
    {
    }

        /// <summary>
        /// Gets or sets the current value internal.
        /// </summary>
        /// <value>The current value internal.</value>
        protected override object CurrentValueInternal
    {
      get
      {
        return (object) null;
      }
      set
      {
      }
    }

        /// <summary>
        /// Gets or sets the start value.
        /// </summary>
        /// <value>The start value.</value>
        public override object StartValue
    {
      get
      {
        return (object) null;
      }
      set
      {
      }
    }

        /// <summary>
        /// Gets or sets the end value.
        /// </summary>
        /// <value>The end value.</value>
        public override object EndValue
    {
      get
      {
        return (object) null;
      }
      set
      {
      }
    }

        /// <summary>
        /// Gets the value for step.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>System.Object.</returns>
        protected override object GetValueForStep(double step)
    {
      return (object) null;
    }
  }
}
