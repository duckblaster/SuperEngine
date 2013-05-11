namespace SuperEngine.Macros

import System
import Boo.Lang.Compiler
import Boo.Lang.Compiler.Ast

macro property:
	canSetFunc as Expression = [| true |]
	
	case [| property $propertyName as $propertyType |]:
		backingField = Boo.Lang.Compiler.Ast.ReferenceExpression("_" + propertyName)
		yield [|
			private $backingField as $propertyType
		|] 
		yield [|
			$propertyName as $propertyType:
				get: 
					value = $backingField
					$onGetFunc
					return value
				set:
					if $backingField != value:
						oldValue = $backingField
						newValue = value
						if $canSetFunc:
							$onSetFunc
							$backingField = newValue
		|]
	
	macro canSet:
		property.canSetFunc = canSet.Body
	
	macro onSet:
		pass
	
	macro onGet:
		pass
