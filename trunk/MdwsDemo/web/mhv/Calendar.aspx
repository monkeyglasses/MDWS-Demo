<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calendar.aspx.cs" Inherits="MdwsDemo.web.mhv.Calendar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">


<title>My HealtheVet  </title><link href="./My HealtheVet_files/body.css" rel="stylesheet" type="text/css"><link href="./My HealtheVet_files/portlet.css" rel="stylesheet" type="text/css"><link href="./My HealtheVet_files/book-leftnav.css" rel="stylesheet" type="text/css"><link href="./My HealtheVet_files/window-plain.css" rel="stylesheet" type="text/css"><link rel="stylesheet" href="https://www.myhealth.va.gov/mhv-portal-web/framework/skins/default/css/book.css"><link rel="stylesheet" href="./My HealtheVet_files/layout.css" type="text/css"><link type="text/css" rel="stylesheet" href="./My HealtheVet_files/window-alert.css"><link rel="stylesheet" href="./My HealtheVet_files/button.css" type="text/css"><link rel="stylesheet" href="./My HealtheVet_files/window.css" type="text/css"><link href="./My HealtheVet_files/book-login.css" rel="stylesheet" type="text/css"><link type="text/css" rel="stylesheet" href="./My HealtheVet_files/book-rightbar.css"><link href="./My HealtheVet_files/custom-mhv.css" type="text/css" rel="stylesheet"><link rel="stylesheet" href="./My HealtheVet_files/custom-mhv-navigation.css" type="text/css"><link type="text/css" rel="stylesheet" href="./My HealtheVet_files/window-leftnav.css"><link href="./My HealtheVet_files/window-rightbar.css" rel="stylesheet" type="text/css"><link rel="stylesheet" href="./My HealtheVet_files/form.css" type="text/css"><link href="https://www.myhealth.va.gov/mhv-portal-web/framework/skins/default/css/treenav.css" rel="stylesheet"><link rel="stylesheet" href="./My HealtheVet_files/custom-mhv-tables.css" type="text/css"><link type="text/css" rel="stylesheet" href="./My HealtheVet_files/window-login.css"><script src="./My HealtheVet_files/util.js" type="text/javascript"></script><script src="./My HealtheVet_files/skin.js" type="text/javascript"></script><script type="text/javascript" src="./My HealtheVet_files/float.js"></script><script type="text/javascript" src="./My HealtheVet_files/delete.js"></script><script src="./My HealtheVet_files/menu.js" type="text/javascript"></script><script type="text/javascript" src="./My HealtheVet_files/leftNav.js"></script><script src="./My HealtheVet_files/menufx.js" type="text/javascript"></script>




	
	<!--[if IE]>
		<link rel="stylesheet" type="text/css" href="/mhv-portal-web/framework/skins/default/css/custom-mhv-ie.css" /> 
	<![endif]-->
	<script type="text/javascript" src="./My HealtheVet_files/foresee-trigger.js"></script>
	<script src="./My HealtheVet_files/foresee-surveydef.js" type="text/javascript" id="foresee-surveydef"></script>
</head>
<body>
    <form id="formMHV" action="Calendar.aspx" method="post" runat="server">
    <div>

<body class="bea-portal-body" onload="initSkin();" id="mhv-cal-tab-month"><div class="bea-portal-body-content">









    
    <div class="bea-portal-body-header">



















 

<!-- START VA BANNER -->
<div id="va-banner-container" class="va-banner-container">
    <a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_pageLabel=healthCalendar&_nfls=false#skip_navigation"><img id="skipNavigation" src="./My HealtheVet_files/spacer.gif" alt="Skip Navigation" name="skipNavigation" width="1" height="1" border="0" align="left"></a>
    <a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_pageLabel=healthCalendar&_nfls=false#skip_navigationLogin"><img id="skipNavigationLogin" src="./My HealtheVet_files/spacer.gif" alt="Skip Navigation To Login" name="skipNavigationLogin" width="1" height="1" border="0" align="left"></a>
    	<div id="errMsgDiv"><!-- this will be filled by defaultNotificationTemplate.jspf if a skip-navigation link is needed --></div>
	<img id="vaBanner" src="./My HealtheVet_files/header-banner-va760.jpg" alt="United States Department  of Veterans Affairs" name="vaBanner" width="760" height="55" border="0" align="bottom">
</div>
<!-- END VA BANNER -->
<!-- START MHV BANNER -->

<%--<form method="GET" target="_blank" action="http://www.index.va.gov/search/va/va_search.jsp">
--%><div id="mhv-banner-container-bg" class="mhv-banner-container-bg">
<div id="mhv-banner-container" class="mhv-banner-container">
    <table class="mhv-banner-layout" width="100%" border="0" cellpadding="0" cellspacing="0" summary="" height="">
        <tbody><tr>
            <td class="va-link-container">
				<!-- VA HOME LINK -->
                <a href="http://www.va.gov/" target="_blank"><img id="" src="./My HealtheVet_files/header-link-vahome.gif" alt="VA Home" name="" width="134" height="24" border="0" align="top"></a>
            </td>
            <td class="mhv-campaign-container">
				<img src="./My HealtheVet_files/spacer.gif" alt="My HealtheVet" width="1" height="1" border="0">
				
				
				
				    <img src="./My HealtheVet_files/header_campaign.jpg" alt="null" width="310" height="59" border="0">
				
				
            </td>
        </tr>
        <tr>
            <td colspan="2" class="mhv-globalnav-menu-container">
				<!-- GLOBAL NAVIGATION --> 
				<div class="mhv-globalnav-menu-single">
                    <a href="http://www1.va.gov/directory/guide/home.asp" class="mhv-globalnav-menu-single-item" target="_blank">VA Facility Locator</a>
                    <a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_nfto=false&_pageLabel=aboutMHV" class="mhv-globalnav-menu-single-item">About MHV</a>
                    <a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_nfto=false&_pageLabel=rssFeeds" class="mhv-globalnav-menu-single-item">RSS Feeds</a>
                    <a href="https://www.myhealth.va.gov/mhv-portal-web/resources/jsp/help.jsp?helpForPortalPage=healthCalendar" target="_mhvHelp" class="mhv-globalnav-menu-single-item">Help</a>
                    <a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_nfto=false&_pageLabel=faqs" class="mhv-globalnav-menu-single-item">FAQs</a>
                    <a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_nfto=false&_pageLabel=contactMHV" class="mhv-globalnav-menu-single-item">Contact MHV</a>
                    <span class="mhv-globalnav-search">Search:
						<input name="SQ" type="hidden" id="searchSelect" value="http://www.myhealth.va.gov">
						<input name="TT" type="hidden" id="TThidden" value="">
												
						<label for="searchtxt"><img id="" src="./My HealtheVet_files/spacer.gif" alt="Search MHV" name="" width="1" height="1" border="0"></label>&nbsp;&nbsp;
						<input type="text" name="QT" id="searchtxt" class="mhv-globalnav-search-field" size="30">&nbsp;&nbsp;
																		
						<label for="searchbtn"><img id="" src="./My HealtheVet_files/spacer.gif" alt="Search MHV" name="" width="1" height="1" border="0"></label>
						<input type="submit" id="searchbtn" class="mhv-globalnav-search-button" value="GO">
					</span>
            	</div>
            </td>
        </tr>
    </tbody></table>
<!-- END MHV BANNER-->	
</div>
</div>
<%--</form>
--%>








    </div>

    








    
    <div class="bea-portal-book-primary">
        









    
    <div class="bea-portal-ie-table-buffer-div">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" summary="">
            <tbody><tr>
                <td class="bea-portal-book-primary-menu-single-container" align="left" nowrap="nowrap">
    <ul class="bea-portal-book-primary-menu-single"><li class="bea-portal-book-primary-menu-single-item"><a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_pageLabel=home&_nfls=false">HOME</a></li><li class="bea-portal-book-primary-menu-single-item"><a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_pageLabel=personalInformation&_nfls=false">PERSONAL INFORMATION</a></li><li class="bea-portal-book-primary-menu-single-item"><a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_pageLabel=pharmacy&_nfls=false">PHARMACY</a></li><li class="bea-portal-book-primary-menu-single-item"><a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_pageLabel=researchHealth&_nfls=false">RESEARCH HEALTH</a></li><li class="bea-portal-book-primary-menu-single-item-active"><a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_pageLabel=getCare&_nfls=false">GET CARE</a></li><li class="bea-portal-book-primary-menu-single-item"><a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_pageLabel=trackHealth&_nfls=false">TRACK HEALTH</a></li><li class="bea-portal-book-primary-menu-single-item"><a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_pageLabel=mhvCommunity&_nfls=false">MHV COMMUNITY</a></li></ul>
                </td>















            </tr>
        </tbody></table>
    </div>
    


        
        <div class="bea-portal-book-primary-content">









    
    <div class="bea-portal-book-invisible">
        









    
    <div class="bea-portal-ie-table-buffer-div">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" summary="">
            <tbody><tr>
                <td class="bea-portal-book-menu-single-container" align="left" nowrap="nowrap">
    <ul class="bea-portal-book-menu-single"><li class="bea-portal-book-menu-single-item"><a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_pageLabel=careGivers&_nfls=false">CARE GIVERS</a></li><li class="bea-portal-book-menu-single-item"><a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_pageLabel=treatmentFacilities&_nfls=false">TREATMENT FACILITIES</a></li><li class="bea-portal-book-menu-single-item"><a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_pageLabel=myCoverage&_nfls=false">MY COVERAGE</a></li><li class="bea-portal-book-menu-single-item-active"><a href="./My HealtheVet_files/My HealtheVet.html">HEALTH CALENDAR</a></li></ul>
                </td>















            </tr>
        </tbody></table>
    </div>
    


        
        <div class="bea-portal-book-content">









    
    <div class="bea-portal-book-page">










    
    <table class="bea-portal-layout-grid" cellspacing="0" summary="">
    
        <tbody><tr>
    
            
            <td class="bea-portal-layout-placeholder-container" width="80%">

<div class="bea-portal-layout-placeholder">








    
    <table class="bea-portal-layout-flow" cellspacing="0" summary="">
        <tbody><tr>
    
            
            <td class="bea-portal-layout-placeholder-container" width="100%">

<div class="bea-portal-layout-placeholder">








    
    <div class="bea-portal-window" width="100%">
    
    <span style="color:black;">
        
             <a name="skip_navigation"></a>             
        
        
    </span>
    
    
        
        
<div class="bea-portal-window-content">



<!-- INSERT NEW CODE HERE!!! -->
Select a date in the calendar to view availability:
<asp:Calendar ID="calendarSelectDate" runat="server" OnSelectionChanged="Click_SelectDate" />

Please select your time slot preference: <asp:DropDownList ID="dropdownAvailableTimes" runat="server" />

<asp:Button ID="buttonMakeAppointment" PostBackUrl="~/web/mhv/Calendar.aspx" Text="Make Appointment"  OnClick="Click_MakeAppointment" runat="server" />





        
		<div id="mhv-cal-portlet-content-outer-wrapper">
			<div id="mhv-cal-portlet-content-wrapper">
                











				<div id="mhv-cal-portlet-content-container">
					<div id="mhv-cal-portlet-content">
                        















                                
					</div>
				</div>
				<div class="mhv-cal-clear"></div>
				<div id="mhv-cal-portlet-content-bottom"></div>
			</div>
		</div>

        <div class="mhv-cal-clear"></div>
    </div>









</div>
    </div>
    



</div>
</td>
            
    
        </tr>
    </tbody></table>
    














</div>
</td>
            
    
            
            <td class="bea-portal-layout-placeholder-container" width="20%">

<div class="mhv-hide-from-print-view">








    
    <table class="bea-portal-layout-flow" cellspacing="0" summary="">
        <tbody><tr>
    
            
            <td class="bea-portal-layout-placeholder-container" width="100%">

<div class="bea-portal-layout-placeholder">







    
    <div class="bea-portal-book">
        
        
        <div class="bea-portal-book-content">









    
    <div class="bea-portal-book-page">










    
    <table class="bea-portal-layout-grid" cellspacing="0" summary="">
    
        <tbody><tr>
    
            
            <td class="bea-portal-layout-placeholder-container">

<div class="bea-portal-layout-placeholder">








    
    <table class="bea-portal-layout-flow" cellspacing="0" summary="">
        <tbody><tr>
    
            
            <td class="bea-portal-layout-placeholder-container" width="100%">

<div class="bea-portal-layout-placeholder">








    
    <div class="bea-portal-window" width="100%">
    
    <span style="color:black;">
        
        
    </span>
    
    
        
        
<div class="bea-portal-window-content">








<div netui:idscope="logoutPortlet_healthCalendar">
	<a name="Logout"></a>
        <h2>Member Logout</h2>
        <div>
            <strong>Logged On As: <asp:Label ID="labelPatientName" runat="server" /></strong><br>
        </div>
        <br>
        <div class="mhv-button-container-align-right"><a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_windowLabel=logoutPortlet_healthCalendar&logoutPortlet_healthCalendar_actionOverride=%2Fgov%2Fva%2Fmed%2Fmhv%2Fusermgmt%2Fportlet%2Flogout%2Flogout&_pageLabel=healthCalendarRightbarLoginPage" class="mhv-input-button">Logout</a></div>
        
</div> 







</div>
    </div>
    



</div>
</td>
            
    
        </tr>
    </tbody></table>
    














</div>
</td>
            
    
        </tr>
    
    </tbody></table>
    



















    </div>
    








        </div>
        
    </div>
    



</div>
</td>
            
    
        </tr>
    </tbody></table>
    














</div>
</td>
            
    
        </tr>
    
    </tbody></table>
    



















    </div>
    








        </div>
        
    </div>
    








        </div>
        
    </div>
    








    
    <div class="bea-portal-body-footer">





<!-- START FOOTER -->
<div id="mhv-footer-container" class="mhv-footer-container">
	<div class="mhv-footer">
    	<span class="mhv-footer-text-blue"><a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_nfto=false&_pageLabel=privacy">Privacy &amp; Security</a> | <a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_nfto=false&_pageLabel=termsAndConditions">Terms &amp; Conditions</a> | <a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_nfto=false&_pageLabel=accessibility">Accessibility</a> | <a href="https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_nfto=false&_pageLabel=siteMap">Site Map</a></span>
	</div>
	<div class="mhv-footer">
		<span class="mhv-footer-text-grey">
		<a href="http://www.whitehouse.gov/" target="_blank">The White House</a> |
		<a href="http://www.usa.gov/" target="_blank">USA.gov</a> |
		<a href="http://www.usafreedomcorps.gov/" target="_blank">USA Freedom Corps</a> |
		<a href="http://www.va.gov/cares/" target="_blank">CARES</a> |
		<a href="http://www.defenselink.mil/" target="_blank">Defense Link</a>
		</span>
	</div>
</div>
<!-- END FOOTER -->







    </div>
    






        </div>









    </div>
    </form>
</body>
</html>
