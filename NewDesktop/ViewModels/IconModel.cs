﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using NewDesktop.Models;

namespace NewDesktop.ViewModels;

/// <summary>
///图标视图模型
/// </summary>
public partial class IconModel : ObservableObject, IDragSource
{

    [ObservableProperty]
    private Icon _model;

    [ObservableProperty]
    private ImageSource  _jumboIcon;
    
    #region 属性绑定

    public string Name
    {
        get => Model.Name;
        set => SetProperty(Model.Name, value, Model, (m, v) => m.Name = v);
    }
    
    public string Path
    {
        get => Model.Path;
        set => SetProperty(Model.Path, value, Model, (m, v) => m.Path = v);
    }
    
    public int Stock
    {
        get => Model.Stock;
        set => SetProperty(Model.Stock, value, Model, (m, v) => m.Stock = v);
    }

    public double X
    {
        get => Model.X;
        set => SetProperty(Model.X, value, Model, (m, v) => m.X = v);
    }

    public double Y
    {
        get => Model.Y;
        set => SetProperty(Model.Y, value, Model, (m, v) => m.Y = v);
    }

    #endregion
    
    // 构造函数增加父集合参数
    public IconModel(Icon model)
    {
        _model = model;
    }

    [RelayCommand]
    private void HandleDoubleClick()
    {
        if (string.IsNullOrWhiteSpace(Model.Path))
            return;

        try
        {
            // 方式1：使用 ProcessStartInfo（推荐，更安全可控）
            var startInfo = new ProcessStartInfo
            {
                FileName = Model.Path,      // 文件路径
                UseShellExecute = true,      // 使用系统 Shell 打开（关联程序）
                Verb = "open"                // 指定操作为"打开"
            };
            System.Diagnostics.Process.Start(startInfo);

            // 方式2：简洁写法（仅适用于 .NET Core 3.1+）
            // System.Diagnostics.Process.Start(new ProcessStartInfo(Model.Path) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            // 处理异常（如文件不存在、无关联程序等）
            Debug.WriteLine($"打开文件失败: {ex.Message}");
            // 可选：显示错误提示
            // MessageBox.Show($"无法打开文件: {Model.Path}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    //#region 拖动

    // 开始拖动时的初始化操作
    public void StartDrag(IDragInfo dragInfo)
    {
        // 获取所有选中的项（支持多选）
        var selectedItems = dragInfo.SourceItems?.Cast<IconModel>().ToList() ?? new List<IconModel> { this };
        // 传递选中的集合
        dragInfo.Data = selectedItems.Count == 1 ? 
            selectedItems.First() : 
            selectedItems.AsEnumerable();
        
        dragInfo.Effects = DragDropEffects.Move;
        // dragInfo.Data = this; // 传递数据模型.Model
        // dragInfo.Effects = DragDropEffects.Move;
    }
    
    // 判断是否允许启动拖动操作（这里始终允许）
    public bool CanStartDrag(IDragInfo dragInfo) => true;

    // 修改DragDrop完成回调
    public void DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo) { }
    
    // 当元素被放置到目标位置时的处理
    public void Dropped(IDropInfo dropInfo) { }

    // 拖动操作被取消时的处理
    public void DragCancelled() { }

    // 异常处理策略
    public bool TryCatchOccurredException(Exception exception) => false;

    //#endregion

}