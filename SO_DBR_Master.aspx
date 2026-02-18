<%@ Page Title="SO_DBR_Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SO_DBR_Master.aspx.cs" Inherits="SO_Appraisal.SO_DBR_Master" %>



<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" />

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-icons/1.10.5/font/bootstrap-icons.min.css" />

    <style>
        .toast-custom {
            position: fixed;
            top: 10px;
            right: 20px;
            width: 300px;
            background-color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            padding: 15px;
        }

        .toast-success {
            border-left: 5px solid #28a745; /* Light green */
        }

        .toast-danger {
            border-left: 5px solid #dc3545; /* Red */
        }

        .form-control {
            text-align: center;
        }

        @media (max-width: 768px) {
            .col-12 {
                text-align: center;
            }

            /*#btnOpenModal {
                width: 100%;
            }
*/
            .grid-wrapper {
                overflow-x: auto;
                -webkit-overflow-scrolling: touch;
            }

                .grid-wrapper table {
                    width: 100%;
                    display: block;
                }

                    .grid-wrapper table thead,
                    .grid-wrapper table tbody {
                        display: table;
                        width: 100%;
                    }
        }

        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }

        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
        }

        .grid-wrapper {
            max-height: 300px;
            max-width: 100%;
            overflow: auto;
        }

        /* Keyframes for progress animation */
        @keyframes progress-animation {
            0% {
                width: 0%;
            }

            50% {
                width: 50%;
            }

            100% {
                width: 100%;
            }
        }

        /* Adjustments for small screens */
        @media (max-width: 768px) {
            .progress-bar-container {
                width: 100%; /* Full width on smaller screens */
                max-width: 100%; /* Ensures the progress bar can stretch to the screen size */
            }
        }

        /* Style the autocomplete dropdown to look like a DropDownList */
        .ui-autocomplete {
            list-style: none;
            margin: 0;
            padding: 0;
            background-color: white;
            border: 1px solid #ccc;
            border-radius: 4px;
            max-height: 200px;
            overflow-y: auto;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
            z-index: 1050;
            width: 50px;
        }

        .ui-menu-item {
            padding: 8px 12px;
            font-size: 14px;
        }

            .ui-menu-item:hover {
                background-color: #007bff;
                color: white;
            }

        /* Hide any default message shown by jQuery UI */
        .ui-helper-hidden-accessible {
            display: none;
        }

        /* Common alert styles */
        .alert-box {
            display: none;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index: 1050; /* Ensure it stays on top */
            max-width: 500px;
            width: 100%;
            background-color: white;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            border-radius: 5px;
            padding: 20px;
        }

        /* Common alert backdrop */
        .alert-backdrop {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0, 0, 0, 0.5);
            z-index: 1049; /* Just below the alert */
        }

        .loader-container {
            display: none; /* Initially hidden */
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index: 1051; /* Ensure it stays on top */
            flex-direction: column;
            align-items: center;
        }

        .loader {
            width: 80px;
            height: 80px;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
            margin-bottom: 20px;
        }

            .loader::before, .loader::after {
                content: "";
                position: absolute;
                border-radius: 50%;
                animation: pulse 1.5s infinite ease-in-out;
            }

            .loader::before {
                width: 80px;
                height: 80px;
                border: 6px solid #ff7eb3;
            }

            .loader::after {
                width: 60px;
                height: 60px;
                border: 6px solid #ff758c;
                animation-delay: 0.75s;
            }

        @keyframes pulse {
            0% {
                transform: scale(1);
                opacity: 1;
            }

            50% {
                transform: scale(1.3);
                opacity: 0.5;
            }

            100% {
                transform: scale(1);
                opacity: 1;
            }
        }

        .loading-text {
            font-size: 18px;
            color: #333;
            font-weight: bold;
            text-align: center;
        }

        /*Floating Label*/
        .floating-label {
            position: relative;
            /* margin-top: 1.5rem; */
            /*margin-bottom: 1.5rem;*/
        }

            .floating-label label {
                position: absolute;
                top: -0.6rem;
                left: 0.75rem;
                background: white;
                padding: 0 4px;
                font-size: 0.8rem;
                color: #3f51b5;
                z-index: 1;
            }

            .floating-label select,
            .floating-label input,
            .floating-label button,
            .floating-label .aspNetDropDown {
                width: 100%;
                /* padding: 0.6rem 0.75rem; */
                font-size: 1rem;
                border: 1px solid #ccc;
                border-radius: 4px;
                background-color: transparent;
                /*appearance: none;*/
                /*padding: 0.6rem 0.75rem;*/
                position: relative;
                z-index: 0;
            }

            .floating-label button {
                text-align: left;
            }

            /* Optional: prevent pointer cursor on readonly textbox */
            .floating-label input[readonly] {
                background-color: #f9f9f9;
                cursor: default;
            }

            .floating-label select:focus,
            .floating-label select:not([value=""]) {
                outline: none;
            }

                .floating-label select:focus + label,
                .floating-label select:not([value=""]) + label {
                    top: -0.6rem;
                    font-size: 0.8rem;
                    color: #3f51b5;
                }

        .disabled-red {
            outline: 2px solid red !important;
            box-shadow: 0 0 5px red !important;
            cursor: not-allowed !important;
        }

        .grid-wrapper {
            max-height: 300px;
            max-width: 100%;
            overflow: auto;
        }

        @media (max-width: 768px) {
            .col-12 {
                text-align: center;
            }

            /*#btnOpenModal {
                width: 100%;
            }
*/
            .grid-wrapper {
                overflow-x: auto;
                -webkit-overflow-scrolling: touch;
            }

                .grid-wrapper table {
                    width: 100%;
                    display: block;
                }

                    .grid-wrapper table thead,
                    .grid-wrapper table tbody {
                        display: table;
                        width: 100%;
                    }
        }

        /* Background image container */
        /*.body-content {
            position: relative;
            z-index: 1;*/ /* content stays above background */
        /*}*/

        /* Background image layer */
        /*.body-content::before {
                content: "";
                position: fixed;*/ /* covers full screen */
        /*top: 0;
                left: 0;
                width: 100vw;
                height: 100vh;
                background-image: url('<%= ResolveUrl("~/Images/so_dbr.png") %>');
                background-repeat: no-repeat;
                background-position: center center;
                background-size: contain;*/ /* responsive */

        /*opacity: 0.12;*/ /* subtle background */
        /*z-index: -999;*/ /* lowest possible */
        /*pointer-events: none;*/ /* clickable elements work */
        /*}*/

        /* MEDIUM and larger screens */
        /*@media (min-width: 768px) {
            .body-content::before {
                background-size: 30% auto;*/ /* ✅ md+ */
        /*}
        }*/
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>

    <script>
        function showToast(message, styleClass) {
            var toast = $('<div class="toast-custom ' + styleClass + '">' + message + '</div>').appendTo('#toastContainer');

            // Show the toast
            toast.fadeIn();

            // Move existing toasts down
            $('.toast-custom').not(toast).each(function () {
                $(this).animate({ top: "+=" + (toast.outerHeight() + 10) }, 'fast');
            });

            // Hide the toast after 3 seconds
            setTimeout(function () {
                toast.fadeOut(function () {
                    // Remove the toast from DOM after fadeOut
                    $(this).remove();

                    // Move remaining toasts up
                    $('.toast-custom').each(function (index) {
                        $(this).animate({ top: "-=" + (toast.outerHeight() + 10) }, 'fast');
                    });
                });
            }, 3000);
        }

        function showNoteAlert() {
            $('#noteAlert').show();
        }

        function hideNoteAlert() {
            $('#noteAlert').hide();
        }
    </script>

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="loader-container" id="loaderContainer">
        <div class="loader"></div>
        <div class="loading-text" id="loadingText">Loading...</div>
    </div>

    <div class="container body-content">
        <h2 style="text-align: center; margin-top: 20px;">SO & Distributor Details</h2>

        <div class="headtag">
            <asp:Label ID="lblUserName" runat="server" Style="color: black; float: right; margin-top: 0px; margin-bottom: -20px; margin-right: 20px"></asp:Label>
        </div>

        <br />
        <br />

        <div class="container" style="text-align: center; align-content: center; align-items: center">

            <!-- Center Search Bar -->
            <%--<div class="row d-flex justify-content-center mt-3 mb-4">
                <div class="col-10 col-md-4">
                    <div class="input-group" style="border: 1px solid #0d6efd; border-radius: 10px; padding: 4px;">
                        <input type="text" id="MainSearch" runat="server" class="form-control border-0" placeholder="Search..." style="box-shadow: none;" />
                        <button type="button" id="SearchBtn" runat="server" class="btn border-0" style="background: none;">
                            <i class="bi bi-search" style="font-size: 20px;"></i>
                        </button>
                    </div>
                </div>
            </div>--%>

            <!-- SEARCH BAR -->
            <%--<div class="row justify-content-center mb-3" >--%>
            <div class="row justify-content-center mb-3" style="text-align: center; align-content: center; align-items: center">
                <div class="col-md-6 d-flex justify-content-center">
                    <div class="input-group" style="max-width: 250px; width: 100%;">
                        <asp:TextBox ID="MainSearch" runat="server" CssClass="form-control rounded-pill"
                            placeholder="Search here..." />
                        <div class="input-group-append">
                            <asp:LinkButton ID="SearchBtn" runat="server" CssClass="btn btn-primary rounded-pill ml-2"
                                OnClick="SearchBtn_Click">
                            <i class="bi bi-search"></i>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <br />
            <div class="row">
                <div class="col-6 col-md-3 mb-2 mb-md-0">
                    <div class="floating-label">
                        <asp:DropDownList ID="StateDrp" runat="server" AutoPostBack="true" Style="display: none;" class="form-control" OnSelectedIndexChanged="StateDrp_SelectedIndexChanged" ClientIDMode="Static">
                        </asp:DropDownList>

                        <input type="text" id="StateSearch" runat="server" class="form-control" placeholder="Enter State" />
                        <label for="StateDrp">State</label>
                        <%--<asp:Label runat="server" Text="From SO" AssociatedControlID="FromSODrp" />--%>
                    </div>
                </div>
                <div class="col-6 col-md-3 mb-2 mb-md-0">
                    <div class="floating-label">
                        <asp:DropDownList ID="AreaDrp" runat="server" AutoPostBack="true" Style="display: none;" class="form-control" OnSelectedIndexChanged="AreaDrp_SelectedIndexChanged">
                            <%--<asp:ListItem Text="Area" Value=""></asp:ListItem>--%>
                        </asp:DropDownList>
                        <%--<label for="ToSODrp">To SO</label>--%>
                        <asp:Label runat="server" Text="Area" AssociatedControlID="AreaDrp" />

                        <input type="text" id="AreaSearch" runat="server" class="form-control" placeholder="Enter Area" />
                    </div>
                </div>
                <div class="col-6 col-md-3 mb-2 mb-md-0">
                    <div class="floating-label">
                        <asp:DropDownList ID="ZoneDrp" runat="server" AutoPostBack="true" Style="display: none;" class="form-control" OnSelectedIndexChanged="ZoneDrp_SelectedIndexChanged">
                            <%--<asp:ListItem Text="Area" Value=""></asp:ListItem>--%>
                        </asp:DropDownList>
                        <%--<label for="ToSODrp">To SO</label>--%>
                        <asp:Label runat="server" Text="Zone" AssociatedControlID="ZoneDrp" />

                        <input type="text" id="ZoneDrpSearch" runat="server" class="form-control" placeholder="Enter Zone" />
                    </div>
                </div>
                <div class="col-6 col-md-3 mb-2 mb-md-0">
                    <div class="floating-label">
                        <asp:DropDownList ID="SODrp" runat="server" AutoPostBack="true" Style="display: none;" class="form-control" OnSelectedIndexChanged="FromSODrp_SelectedIndexChanged">
                        </asp:DropDownList>

                        <asp:Label runat="server" Text="SO" AssociatedControlID="SODrp" />

                        <input type="text" id="SOSearch" runat="server" class="form-control" placeholder="Enter SO" />
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-6 col-md-3 mb-2 mb-md-0">
                </div>
                <div class="col-6 col-md-3 mb-2 mb-md-0">
                </div>
                <div class="col-6 col-md-3 mb-2 mb-md-0">
                </div>
                <div class="col-6 col-md-3 mb-2 mb-md-0">
                    <asp:Button ID="Fetch" runat="server" Text="Fetch" CssClass="form-control btn btn-success" OnClick="Fetch_Click" OnClientClick="showLoader()" />
                </div>
            </div>
            <%--<asp:Button ID="Download" runat="server" Text="Download" CssClass="btn btn-primary" OnClick="Download_Click" OnClientClick="showLoader()" />--%>


            <!-- GridView -->
            <div class="row mt-3">
                <div class="col-12">
                    <div class="grid-wrapper">
                        <asp:GridView ID="ResultGrid" runat="server" AutoGenerateColumns="true"
                            CssClass="table table-bordered form-group" Style="text-align: center;">
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
    <asp:HiddenField ID="hdnBusinessType" runat="server" />
    <asp:HiddenField ID="hdnRole" runat="server" />
    <asp:HiddenField ID="hfSelectedRowData" runat="server" />


    <%--Script for Dropdown Auto Search--%>
    <script>
        $(document).ready(function () {

            // --- State autocomplete ---
            (function () {
                var ddl = $('#<%= StateDrp.ClientID %>');
                var searchBox = $('#<%= StateSearch.ClientID %>');

                var options = [];
                ddl.find("option").each(function () {
                    var text = $(this).text();
                    var value = $(this).val();
                    if (value) {
                        options.push({ label: text, value: value });
                    }
                });

                searchBox.autocomplete({
                    source: options,
                    minLength: 1,
                    select: function (event, ui) {
                        searchBox.val(ui.item.label);
                        ddl.val(ui.item.value);

                        // Trigger SelectedIndexChanged of StateDrp
                        __doPostBack('<%= StateDrp.UniqueID %>', '');

                        return false;
                    }
                });
            })();


            // --- Area autocomplete ---
            (function () {
                var ddl = $('#<%= AreaDrp.ClientID %>');
                var searchBox = $('#<%= AreaSearch.ClientID %>');

                var options = [];
                ddl.find("option").each(function () {
                    var text = $(this).text();
                    var value = $(this).val();
                    if (value) {
                        options.push({ label: text, value: value });
                    }
                });

                searchBox.autocomplete({
                    source: options,
                    minLength: 1,
                    select: function (event, ui) {
                        searchBox.val(ui.item.label);
                        ddl.val(ui.item.value);

                        // Trigger SelectedIndexChanged of AreaDrp
                        __doPostBack('<%= AreaDrp.UniqueID %>', '');

                        return false;
                    }
                });
            })();

            // --- Zone autocomplete ---
            (function () {
                var ddl = $('#<%= ZoneDrp.ClientID %>');
                var searchBox = $('#<%= ZoneDrpSearch.ClientID %>');

                var options = [];
                ddl.find("option").each(function () {
                    var text = $(this).text();
                    var value = $(this).val();
                    if (value) {
                        options.push({ label: text, value: value });
                    }
                });

                searchBox.autocomplete({
                    source: options,
                    minLength: 1,
                    select: function (event, ui) {
                        searchBox.val(ui.item.label);
                        ddl.val(ui.item.value);

                        // Trigger SelectedIndexChanged of ZoneDrp
                        __doPostBack('<%= ZoneDrp.UniqueID %>', '');

                        return false;
                    }
                });
            })();

            // --- FromSO autocomplete ---
            (function () {
                var ddl = $('#<%= SODrp.ClientID %>');
                var searchBox = $('#<%= SOSearch.ClientID %>');

                var options = [];
                ddl.find("option").each(function () {
                    var text = $(this).text();
                    var value = $(this).val();
                    if (value) {
                        options.push({ label: text, value: value });
                    }
                });

                searchBox.autocomplete({
                    source: options,
                    minLength: 1,
                    select: function (event, ui) {
                        // set visible text
                        searchBox.val(ui.item.label);
                        // set hidden dropdown selected value
                        ddl.val(ui.item.value);
                        // 🔴 this is what fires FromSODrp_SelectedIndexChanged
                        __doPostBack('<%= SODrp.UniqueID %>', '');
                        return false;
                    }
                });
            })();

            <%--// --- ToSODrp autocomplete ---
            (function () {
                var ddl = $('#<%= ToSODrp.ClientID %>');
                var searchBox = $('#<%= ToSODrpSearch.ClientID %>');

                var options = [];
                ddl.find("option").each(function () {
                    var text = $(this).text();
                    var value = $(this).val();
                    if (value) {
                        options.push({ label: text, value: value });
                    }
                });

                searchBox.autocomplete({
                    source: options,
                    minLength: 1,
                    select: function (event, ui) {
                        // set visible text
                        searchBox.val(ui.item.label);
                        // set hidden dropdown selected value
                        ddl.val(ui.item.value);
                        // 🔴 this is what fires ToSODrp_SelectedIndexChanged
                        __doPostBack('<%= ToSODrp.UniqueID %>', '');
                        return false;
                    }
                });
            })();

            // --- To Zone autocomplete ---
            (function () {
                var ddl = $('#<%= ToZoneLoadDrp.ClientID %>');
                var searchBox = $('#<%= ToZoneLoadDrpSearch.ClientID %>');

                var options = [];
                ddl.find("option").each(function () {
                    var text = $(this).text();
                    var value = $(this).val();
                    if (value) {
                        options.push({ label: text, value: value });
                    }
                });

                searchBox.autocomplete({
                    source: options,
                    minLength: 1,
                    select: function (event, ui) {
                        searchBox.val(ui.item.label);
                        ddl.val(ui.item.value);

                        // Trigger SelectedIndexChanged of ToZoneLoadDrp
                        __doPostBack('<%= ToZoneLoadDrp.UniqueID %>', '');

                        return false;
                    }
                });
            })();--%>


        });
    </script>

    <%-- Script for search button in Modal --%>
    <%--<script type="text/javascript">
        $(document).ready(function () {
            $("#txtSearch").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#<%= DistModal.ClientID %> tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>--%>

    <%--script for checkboxes in Distributor Modal to remain atleast one Distributor--%>
    <script type="text/javascript">

        // Function to handle checkbox click
        function handleCheckboxClick(checkbox) {
            const checkboxes = document.querySelectorAll('.rowCheckbox');

            const checkedCount = Array.from(checkboxes).filter(cb => cb.checked).length;

            checkboxes.forEach(cb => {
                if (!cb.checked && checkedCount === checkboxes.length - 1) {

                    cb.disabled = true;
                    cb.classList.add("disabled-red");  // highlight uncheckable

                    showToast("At least one Distributor will remain in Transfer case", "toast-danger");

                } else {

                    cb.disabled = false;
                    cb.classList.remove("disabled-red");

                }
            });
        }

        // This function is triggered when the page or modal is loaded to ensure proper checkbox states
        function setCheckboxState() {
            const checkboxes = document.querySelectorAll('.rowCheckbox');

            if (checkboxes.length === 1) {
                checkboxes[0].disabled = true;
                checkboxes[0].classList.add("disabled-red");
            } else {
                checkboxes.forEach(cb => {
                    cb.disabled = false;
                    cb.classList.remove("disabled-red");
                });
            }
        }

        // Ensure proper checkbox state on modal open
        window.onload = function () {
            setCheckboxState();
        }
    </script>

    <%--script to handle alert--%>
    <script>
        function openTransferModal() {
            $('#transferModalCenter').modal('show');
        }

        function transferAlert() {
            document.getElementById("TransferAlert").style.display = "block";
        }

        function hideAlert() {
            document.getElementById("TransferAlert").style.display = "none";
        }
    </script>

    <script>
        const messages = [
            "Please wait...",
            "Processing your request...",
            "Almost there...",
            "Just a moment...",
            "Fetching data...",
            "Hang tight...",
            "Thanks for your patience...",
            "We appreciate your wait..."
        ];

        let index = 0;
        let interval;

        function showLoader() {
            document.getElementById("loaderContainer").style.display = "flex"; // Show loader

            // Start changing messages
            interval = setInterval(() => {
                document.getElementById("loadingText").innerText = messages[index];
                index = (index + 1) % messages.length;
            }, 3000);

            // Simulate some process (hide loader after 10s for demo)
            //setTimeout(() => {
            //    hideLoader();
            //}, 10000);
        }

        function hideLoader() {
            clearInterval(interval);
            document.getElementById("loaderContainer").style.display = "none"; // Hide loader
        }
    </script>

</asp:Content>
