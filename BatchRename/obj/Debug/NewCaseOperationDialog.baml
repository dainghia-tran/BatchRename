   M S B A M L   `   `   `  ����F  BBatchRename, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:BatchRenameBatchRename  5   	   "    BatchRename.ChangeCaseDialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:BatchRename     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Icon$" Assets/app_configuration.png��5	   	    � Title$ Change Case configuration��5
   	   $	��220��6+   $	��330��68    � 
ResizeMode$ NoResize=�6D   .��5      �� .+�5   
   �� -   5         ��Name$ CaseRadioButtons��5       �� Left$ 43��6-   
 �� Top$ 23��6>   $��0q�6N   .��5      �� .+�5      �� $��25��6   %#Choose the case you want to change:6M   	� 5      -   5        $��CaseRadioButton1��5       	� 	GroupName$
 Case��67   $��Lower��6H    d� 	IsChecked$
 TrueY�6X   $��0,0,0,10q�6i   	� 5      -   5        $��CaseRadioButton2��5      $
 Case��67   $��Upper��6H   $��0,0,0,10q�6X   	� 5      -   5        $��CaseRadioButton3��5      $
 Case��67   $��Upper First Letter��6H   $��0,0,0,10q�6e   5      5      �� 5   
   -   5        $��
SaveButton��5      $ 43��6<   $	 137��6M   $��75��6^   $��28��6i   .��6v   Save5      �� 5   
   -   5        $��CancelButton��5      $	 166��6%   $	 137��67   $��75��6H   $��28��6S   	 �� IsCancel	 . .��6o   Cancel5      5      5      