   M S B A M L   `   `   `  ����F  BBatchRename, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:BatchRenameBatchRename  5   	   %    BatchRename.MoveOperationDialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:BatchRename     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Icon$" Assets/app_configuration.png��5	   	    � Title$ MoveOperationDialog��5
   	   $	��200��6%   $	��330��62    � 
ResizeMode$ NoResize=�6>   .��5      �� .+�5   
   ��  �� Left$ 45��6   
 �� Top$ 25��6%   $��Moved string length:��65   6S   �� 5   
   -   5        $	��Num��5      $	 179��6   $ 25��60   $��64��6@   $��18��6K   6Y   �� 5   
   $#��String will move to position:��6   $ 44��69   $ 56��6J   6\   �� 5   
    �� Orientation$ 
Horizontal=�6   $ 45��6.   $ 77��6?   .+�5      	� -   5         	�Name$ RadioButton1��5       	� 	GroupName$ PositionRadioButtons��60   $��Front��6Q   $��	0, 0,50,0q�6a   	� 5      -   5        $ RadioButton2��5      $ PositionRadioButtons��60   $
��Last��6Q   5      �� 5   
   -   5        $��
SaveButton��5      $ 45��6<   $	 110��6M   $��75��6^   $��28��6i   .��6v   Save5      �� 5   
   -   5        $��CancelButton��5      $	 168��6%   $	 110��67   $��75��6H   $��28��6S   	 �� IsCancel	 . .��6o   Cancel5      5      5      