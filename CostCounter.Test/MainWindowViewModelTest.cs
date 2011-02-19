using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CostCounter.ViewModel;
using NUnit.Framework;

namespace CostCounter.Test
{
    [TestFixture]
    public class MainWindowViewModelTest
    {
        [Test]
        public void 開始してないと停止コマンドは使えない()
        {
            var model = new MainWindowViewModel();
        }
    }
}
