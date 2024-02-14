Formal View of Profile Content
Link to the formal definition views for the vital signs listed in this table.

• The table below represents an expansion of the US Core/FHIR Core Vital Signs requirements, their required codes, and UCUM units of measure codes used for representing vital signs observations. Any system supporting any of these vital signs concepts must represent them using at least these codes.

• The first column of this table links to the formal views of the individual profile for each vital sign.

• If a more specific code or another code system is recorded or required, implementers must support both the values (LOINC) listed and the translated code - e.g. method specific LOINC codes, SNOMED CT concepts, system specific (local) codes.

• In addition, the implementer may choose to provide alternate codes in addition to the standard codes defined here. The examples illustrate using other codes as translations.

# Profile Name	LOINC Code	LOINC Name and Comments	UCUM Unit Code	Examples
## Vital Signs Panel  	  85353-1  	Vital signs, weight, height, head circumference, oxygen saturation and BMI panel - It represent a panel of vital signs listed in this table. All members of the panel are optional and note that querying for the panel may miss individual results that are not part of the actual panel. When used, Observation.valueQuantity is not present; instead, related links (with type=has-member) reference the vital signs observations (e.g. respiratory rate, heart rate, BP, etc.). This code replaces the deprecated code 8716-3 - Vital signs which is used in the Argonaut Data Query Implementation Guide.	-	Vital Signs Panel Example  
## Blood Pressure Panel  	  85354-9  	Blood pressure panel with all children optional - This is a component observation. It has no value in Observation.valueQuantity and contains at least one component, Systolic blood pressure.	-	Blood Pressure Panel Example  
## Average Blood Pressure  	  96607-7  	Blood pressure panel unspecified time mean. This observations will have components of Systolic (LOINC code 96608-5) and Diastolic (LOINC code 96609-3) average blood pressures over an unspecified period of time.	-	Average Blood Pressure Example  
## 24 hour blood pressure  	  97844-5  	Blood pressure panel 24 hour mean. This observation has components of Systolic (LOINC code 8490-5) and Diastolic (LOINC code 8472-3) average blood pressures over a 24 hour period.	-	24 hour blood pressure Example  
## Respiratory Rate  	  9279-1  	Respiratory rate	/min	Respiratory Rate example  
## Heart Rate  	  8867-4  	Heart rate	/min	Heart Rate example  
## Oxygen Saturation by Pulse Oximetry  	  59408-5  	Oxygen saturation in Arterial blood by Pulse oximetry	%	Oxygen Saturation example  
### Oxygen Saturation  	  2708-6  	Oxygen saturation in Arterial blood	%	Oxygen Saturation example  
## Body Temperature  	  8310-5  	Body temperature	Cel, [degF]	Body Temperature example  
## Body Height  	  8302-2  	Body height	cm, [in_i]	Body Height example  
## Body Length  	  8306-3  	Body height -- lying	cm, [in_i]	Body Length example  
## Head Circumference  	  8287-5  	Head Occipital-frontal circumference by Tape measure	cm, [in_i]	Head ##Circumference example  
### Body Weight  	  29463-7  	Body weight	g, kg,[lb_av]	Body Weight example  
### Body Mass Index  	  39156-5  	Body mass index (BMI) [Ratio]	kg/m2	Body Mass Index example  
