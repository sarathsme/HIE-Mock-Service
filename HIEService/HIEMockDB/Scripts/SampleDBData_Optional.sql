/* 
This script contains some Sample data that may be used to populate the HIE database optionally.
Otherwise custom data can be directly inserted into the Database also.
*/

IF NOT EXISTS (
		SELECT 1
		FROM Patient
		WHERE SSN = '123-55-6565'
		)
BEGIN
	INSERT INTO [dbo].[Patient] ([FirstName],[MiddleName],[FamilyName],[Gender],[DateOfBirth],[SSN],[EMPID],[IPHRID])
	VALUES ('Sarath','S','Menon','M','1993-07-13','123-55-6565','2000417677','12123')

	INSERT INTO [dbo].[PatientAddress] ([PatientID],[StreetAddressLine],[City],[StateOrProvince],[County],[PostalCode])
	VALUES (
		(
		SELECT PatientId
		FROM Patient
		WHERE SSN = '123-55-6565'
		)
		,'Kodakara','Thrissur','Kerala','Kavil','680684'
			)

	INSERT INTO [dbo].[PatientAddress] ([PatientID],[StreetAddressLine],[City],[StateOrProvince],[County],[PostalCode])
	VALUES 
	(
		(
		SELECT PatientId
		FROM Patient
		WHERE SSN = '123-55-6565'
		)
		,'Cunningham Rd','Bangalore','Karnataka','Vasanth Nagar','560052'
	)

	INSERT INTO [dbo].[PatientContactNumber] ([PatientID],[ContactType],[ContactNumber])
	VALUES 
	(
		(
		SELECT PatientId
		FROM Patient
		WHERE SSN = '123-55-6565'
		)
	,'H','7829349046'
	)

	INSERT INTO [dbo].[PatientContactNumber] ([PatientID],[ContactType],[ContactNumber])
	VALUES 
	(
		(
		SELECT PatientId
		FROM Patient
		WHERE SSN = '123-55-6565'
		)
	,'W','9048461414'
	)


INSERT INTO [dbo].[PatientDocuments] ([DocumentUniqueId],[PatientID],[Document],[IsDeprecated])
VALUES (
	'D4382AEB-D796-4CB0-83F4-7D4A31D5C29B'
	,(
		SELECT PatientId
		FROM Patient
		WHERE SSN = '123-55-6565'
		)
	,'<ClinicalDocument xmlns="urn:hl7-org:v3" classCode="DOCCLIN" moodCode="EVN">   <typeId extension="POCD_HD000040" root="2.16.840.1.113883.1.3" />   <templateId root="1.3.6.1.4.1.19376.1.2.20" />   <id extension="M0000270601JS" root="2.16.840.1.113883.3.189.1.74" />   <code code="18726-0" codeSystem="2.16.840.1.113883.6.1" codeSystemName="LOINC" displayName="Imaging" />   <title>ABDOMEN 1 VIEW (JSH Sce624)</title>   <effectiveTime value="201508031228-0400" />   <confidentialityCode code="R" codeSystem="2.16.840.1.113883.5.25" />   <languageCode code="en-US" />   <recordTarget>     <patientRole>       <id assigningAuthorityName="Jersey Shore" extension="JM00JSH05" root="2.16.840.1.113883.3.189.1.74" />       <addr>         <streetAddressLine>KEYHIETEN STREET</streetAddressLine>         <city>ZZGHSBURG</city>         <state>PA</state>         <postalCode>00010</postalCode>         <country>USA</country>       </addr>       <patient>         <name>           <given>KEYHIETEN</given>           <family>ZZGSH</family>         </name>         <administrativeGenderCode code="M" codeSystem="2.16.840.1.113883.5.1" />         <birthTime value="18911010" />       </patient>     </patientRole>   </recordTarget>   <author>     <templateId root="1.3.6.1.4.1.19376.1.2.20.1" />     <time value="201508031228-0400" />     <assignedAuthor>       <id extension="KHADMIN" root="2.16.840.1.113883.3.189.1.74" />       <assignedPerson>         <name>           <prefix nullFlavor="NI" />           <given>KeyHIE</given>           <family>Admin</family>         </name>       </assignedPerson>       <representedOrganization>         <id extension="JSH" root="2.16.840.1.113883.3.189.1.74" />         <name>Jersey Shore</name>       </representedOrganization>     </assignedAuthor>   </author>   <author>     <templateId root="1.3.6.1.4.1.19376.1.2.20.2" />     <time value="201508031228-0400" />     <assignedAuthor>       <id root="2.16.840.1.113883.3.189.1.74" />       <assignedAuthoringDevice>         <code code="WSD" codeSystem="1.2.840.10008.2.16.4" displayName="Workstation" />         <manufacturerModelName>JSHCLIN</manufacturerModelName>         <softwareName>JSHCLIN</softwareName>       </assignedAuthoringDevice>       <representedOrganization>         <id root="2.16.840.1.113883.3.189.1.74" />         <name>Jersey Shore</name>         <addr>           <streetAddressLine>1020 Thompson St.</streetAddressLine>           <city>Jersey Shore</city>           <state>PA</state>           <postalCode>17740</postalCode>           <country nullFlavor="NI" />         </addr>       </representedOrganization>     </assignedAuthor>   </author>   <dataEnterer>     <templateId root="1.3.6.1.4.1.19376.1.2.20.3" />     <time value="201508031228-0400" />     <assignedEntity>       <id extension="null" root="2.16.840.1.113883.3.189.1.74" />       <assignedPerson>         <name>           <prefix nullFlavor="NI" />           <given nullFlavor="NI" />           <family nullFlavor="NI" />         </name>       </assignedPerson>     </assignedEntity>   </dataEnterer>   <custodian>     <assignedCustodian>       <representedCustodianOrganization>         <id extension="JSH" root="2.16.840.1.113883.3.189.1.74" />         <name>Jersey Shore</name>         <addr>           <streetAddressLine>1020 Thompson St.</streetAddressLine>           <city>Jersey Shore</city>           <state>PA</state>           <postalCode>17740</postalCode>           <country>USA</country>         </addr>       </representedCustodianOrganization>     </assignedCustodian>   </custodian>   <documentationOf>     <serviceEvent>       <effectiveTime>         <low nullFlavor="NI" />         <high nullFlavor="NI" />       </effectiveTime>     </serviceEvent>   </documentationOf>   <component>     <nonXMLBody>       <text mediaType="text/plain" representation="B64">IEpTSCAtIFRleHQgZG9jdW1lbnQgZm9yIFNjZW5hcmlvIDYyNA0KIEplcnNleSBTaG9yZSBIb3NwaXRhbA0KICMjIyBTdHJlZXQgQWRkcmVzcyAgDQogQ2l0eSBQQSwgMDAwMDAgIA0KICBQaG9uZSAwMDAtMDAwLTAwMDAgICAgDQogICAgDQogICAgIERJQUdOT1NUSUMgSU1BR0lORyAgICAgICANCiBTaWduZWQgIA0KICANClBhdGllbnQ6IFpaR0hTLEtFWUhJRU5URU4gICAgICAgICAgICAgTVJOOiBKTTAwSlNIMDUgICAgICAgIEFjY3QgIzogIyMjIyMjIyMjIyMjICANCkRPQjogMTAvMTAvMTg5MSAgICAgICAgICAgICAgICAgIEFETSBEYXRlOiAwOC8xNS8xNSAgICAgICAgIERhdGUgb2YgU2VydmljZSAjOiAwOC8xNS8xNSAgDQpGYW1pbHkgcGh5c2ljaWFuOiBSYW5kb20gUGh5c2ljaWFuLCBNRCAgDQpPcmRlcmluZyBwaHlzaWNpYW46ICBSYW5kb20gUGh5c2ljaWFuLCBNRCAgIA0KTG9jYXRpb246IFRQMDAwMSAgDQpSQURJT0xPR1kgIA0KICANCiAgDQogIA0KQUJET01FTiAxIFZJRVcgIA0KICAgDQogSW5kaWNhdGlvbjogSGVtYXR1cmlhICANCiAgIA0KIFRlY2huaXF1ZTogRnJvbnRhbCB2aWV3cyBvZiB0aGUgYWJkb21lbiB3ZXJlIHBlcmZvcm1lZC4gIA0KICAgDQogRmluZGluZ3M6IFRoZSBib3dlbCBnYXMgcGF0dGVybiBpcyBub25vYnN0cnVjdGVkIGFuZCBub25kaWxhdGVkLiBUaGVyZSBpcyBhIG1vZGVyYXRlIGFtb3VudCBvZiANCnJldGFpbmVkIHN0b29sIGluIHRoZSBjb2xvbi4gVHdvIHNtYWxsIG5vbnNwZWNpZmljIGNhbGNpZmljYXRpb25zIGFyZSBub3RlZCBpbiB0aGUgbGVmdCBoZW1pcGVsdmlzLCANCm1lYXN1cmluZyBhcHByb3hpbWF0ZWx5IDIgYW5kIDMgbW0gaW4gc2l6ZS4gVGhlcmUgaXMgYSBwb3NzaWJsZSA1IG1tIGNhbGNpZmljYXRpb24gcHJvamVjdGluZyBvdmVyIA0KdGhlIGV4cGVjdGVkIGxvY2F0aW9uIG9mIHRoZSByaWdodCBraWRuZXkuIEV2YWx1YXRpb24gb2YgdGhlIGtpZG5leXMgaXMgbGltaXRlZCBkdWUgdG8gb3Zlcmx5aW5nIA0KYm93ZWwgY29udGVudHMuIFRoZSB2aXN1YWxpemVkIG9zc2VvdXMgc3RydWN0dXJlcyBhcmUgaW50YWN0LiAgDQogICANCiBJbXByZXNzaW9uOiAgDQogMS4gTGltaXRlZCB2aXN1YWxpemF0aW9uIGR1ZSB0byBvdmVybHlpbmcgYm93ZWwgY29udGVudHMuICANCiAyLiBQb3NzaWJsZSA1IG1tIHJpZ2h0LXNpZGVkIGludHJhcmVuYWwgY2FsY3VsdXMuIE5vbnNwZWNpZmljIGNhbGNpZmljYXRpb25zIGluIHRoZSBsZWZ0IGhlbWlwZWx2aXMNCiwgcG9zc2libHkgcmVwcmVzZW50aW5nIHBobGVib2xpdGhzIG9yIGRpc3RhbCB1cmV0ZXIgc3RvbmVzLiAgDQogICANCiAgICAgICAgICAgICAgICAgRGljdGF0ZWQgQnk6UmFuZG9tIERpY3RhdG9yLCBNRCAgDQogICAgICAgICAgICAgICAgIEVsZWN0cm9uaWNhbGx5IFNpZ25lZCBCeTogUmFuZG9tIERpY3RhdG9yLCBNRCAgDQogICAgICAgICAgICAgICAgIFNpZ25lZCBEYXRlL1RpbWU6MDgvMTUvMTUgMjQwMSAgDQogICAgICAgICAgICAgICAgICAgDQogIA0KVHJhbnNjcmlwdGlvbmlzdDogICAgIA0KRGljdGF0ZWQgRGF0ZTogIDA4LzE1LzE1IDI0MDEgICAgIA0KVHJhbnNjcmliZWQgRGF0ZS9UaW1lOiAgIA0KICANCkNDOiBTb21lb25lIEltcG9ydGFudCwgTUQgIDsgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg</text>     </nonXMLBody>   </component> </ClinicalDocument>'
	,0
	)

END
GO

IF NOT EXISTS (
		SELECT 1
		FROM Patient
		WHERE SSN = '321-54-6546'
		)
BEGIN
	INSERT INTO [dbo].[Patient] ([FirstName],[FamilyName],[Gender],[DateOfBirth],[SSN],[EMPID],[IPHRID])
	VALUES ('Akhil','Achuthan','M','1992-04-15','321-54-6546','2001786635','12124')
END
GO