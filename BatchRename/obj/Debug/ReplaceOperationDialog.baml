   M S B A M L   `   `   `  ����F  BBatchRename, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:BatchRenameBatchRename  5   	   %    BatchRename.ReplaceStringDialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:BatchRename     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Icon$" Assets/app_configuration.png��5	   	    � Title$" Replace String configuration��5
   	   $	��200��6.   $	��330��6;    � 
ResizeMode$ NoResize=�6G   .��5      �� .+�5   
   �� $
��From��6    �� Left$ 33��6    
 �� Top$ 32��61   �� 5   
   -   5        $��FromTextBox��5      $	��189��6&   $ 75��62   $ 30��6C   6U   �� 5   
   $��To��6   $ 33��6   $ 65��6/    J� RenderTransformOrigin$ 0.79,-1.429&�6?   5      �� 5   
   -   5        $��	ToTextBox��5      $ 75��6$   $ 65��65   $	��189��6E   6S   �� 5   
   -   5        $��
SaveButton��5      $ 76��6<   $	 107��6M   $��75��6^   $��28��6i   .��5      Save5      �� 5   
   -   5        $��CancelButton��5      $	 191��6%   $	 107��67   $��75��6H   $��28��6S    �� IsCancel . .��5      Cancel5      5      5      