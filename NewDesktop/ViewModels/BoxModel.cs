﻿using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using GongSolutions.Wpf.DragDrop;
using NewDesktop.Models;

namespace NewDesktop.ViewModels;

/// <summary>
/// 桌面盒子视图模型
/// </summary>
public partial class BoxModel : ObservableObject, IDropTarget
{
    [ObservableProperty]
    private Box _model;
    // private readonly Shelf _model;

    [ObservableProperty]
    private ObservableCollection<IconModel> _products = new();

    public double X
    {
        get => Model.X;
        set => SetProperty<Box, double>(Model.X, value, Model, (m, v) => m.X = v);
    }

    public double Y
    {
        get => Model.Y;
        set => SetProperty<Box, double>(Model.Y, value, Model, (m, v) => m.Y = v);
    }

    public double Width
    {
        get => Model.Width;
        set => SetProperty<Box, double>(Model.Width, value, Model, (m, v) => m.Width = v);
    }

    public double Height
    {
        get => Model.Height;
        set => SetProperty<Box, double>(Model.Height, value, Model, (m, v) => m.Height = v);
    }

    public BoxModel(Box model)
    {
        _model = model;
        foreach (var product in _model.Products) Products.Add(new IconModel(product));
    }
    public string Name
    {
        get => Model.Name;
        set => SetProperty<Box, string>(Model.Name, value, Model, (m, v) => m.Name = v);
    }

    // 拖动到目标区域时触发
    public void DragOver(IDropInfo dropInfo)
    {
        // 检查是否是 Icon 或 IconModel 类型
        if (dropInfo.Data is IconModel || dropInfo.Data is Icon)
        {
            dropInfo.Effects = DragDropEffects.Move;
            //dropInfo.DestinationText = $"移动到 {Model}";
            dropInfo.DestinationText = $"{Name}";
        }
    }

    // 放置时触发
// BoxModel.cs
    public void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is IconModel iconModel)
        {
            // 从原集合移除（安全写法）
            if (dropInfo.DragInfo.SourceCollection is IList source)
            {
                source.Remove(iconModel);
            }

            // 直接添加现有实例（不需要克隆）
            Model.Products.Add(iconModel.Model);
            Products.Add(iconModel);
        }
    } 
}