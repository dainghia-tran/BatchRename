   M S B A M L   `   `   `  ����F  BBatchRename, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:BatchRenameBatchRename  5   	   $    BatchRename.ChangesAlertDialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:BatchRename     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$ There are these changes��5	   	   $	��400��6)   $
��Auto��65    � 
ResizeMode$ NoResize=�6C    � Icon$" Assets/app_configuration.png��5
   	    � 	Resources 5         ListViewItemStyle �� 
TargetType �� Property �� Value �� Triggers	 P� Property
 P� Value��%�  &	        �� 5   
   " ��6*   .�5      ��  � ��
 �   �6+   �� 5       � ��$ 50��6'    5      P� 5      	 �@J�IsMouseOver
 . . �5      ��  � �� �#dbdbdb63   �� 5       � �� �#9c9c9c64   5      P� 5      	 �@��
IsSelected
 . . �5      ��  � �� �#9c9c9c63   �� 5       � �� �#dbdbdb64   �� 5       � ��
 �����63   5      5      5      .��5      ]� $��Center=�6   $	��300��6-   $
��0,30q�69   ��. .+�5   
   ��  �� TextAlignment$ Center=�6   $)��#The following names will be changed��6+   $	��Top=�6V   $��0,0,0,20q�6k   �� 5   
    �� Orientation$ 
Horizontal=�6   $��Bottom=�6.   $��Center=�6F   $��	0,20,0,20q�6c   .+�5      �� -   5        $��OK��5       �� IsCancel .  $��75��6H   $��28��6S   $��0,0,15,0q�6_   6s   �� 5       $��Cancel��6    . $��75��66   $��28��6A   $��15,0,0,0q�6M   6a   5!      �� 5"   
   -   5         ��Name$ ChangedFilesList��5"      #��[  5#      
������ 
���������5&      �� 5'      .R�5(      �� ��6    �� 5)      5+      5,      5-      