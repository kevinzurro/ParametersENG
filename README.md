# Welcome to ParametersENG

ParametersENG is an add-in for Revit 2020 in which you can isolate elements on screen based on the value of a parameter.

<div align="center">
  <img align="center" src="https://github.com/user-attachments/assets/536d222a-a07e-489d-a5bf-829ddde7bd3c">
</div>


## Install

You can [download](https://github.com/kevinzurro/ParametersENG/releases/tag/v2020.1) the files in zip format and paste them into the folder.

`C:\ProgramData\Autodesk\Revit\Addins\2020`

## Functions

* Get all parameters of the document.
* Search elements with specified parameter values.
* Temporarily isolate elements in the current view.
* Select filtered items.

## Restrict

The searched value refers to the [type of parameter](https://www.revitapidocs.com/2020/3dbebcb8-792b-a3dd-fe63-faaa05704f3c.htm) that Revit accepts and they are:
* Integer.
* Double.
* String.
* ElementId.

Elements are only temporarily isolated in the following views:
* Floor Plans.
* Reflected Ceiling Plans.
* 3D Views.
To filter values ​​of type [interger](https://www.revitapidocs.com/2020/507608fe-47fc-1441-acdc-5ce9c3c5da03.htm) or [double](https://www.revitapidocs.com/2020/8831936d-965b-ec90-7e96-b2933c80b88e.htm), the number zero "0" must be placed as the parameter value.
To search for parameters that refer to other [elements](https://www.revitapidocs.com/2020/3e05f5e6-72a2-f633-3740-93feecee8156.htm), you must enter the ID of the element you are searching for.

## Language

The addin is in the English language.
