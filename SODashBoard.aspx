<%@ Page Title="SODashBoard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SODashBoard.aspx.cs" Inherits="SO_Appraisal.SODashBoard" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" />

    <%--    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-icons/1.10.5/font/bootstrap-icons.min.css" />--%>

    <style>
        /* Solid CSS so styles work even with CDN Tailwind (no @apply needed) */
        .card {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            padding: 1.5rem;
            background: #fff;
            border-radius: 0.5rem;
            box-shadow: 0 1px 2px rgba(0,0,0,.05), 0 1px 1px rgba(0,0,0,.04);
            transition: background-color .2s ease, box-shadow .2s ease, transform .1s ease;
            text-align: center;
            font-weight: 600;
            color: #374151;
        }

            .card:hover {
                box-shadow: 0 10px 15px rgba(0,0,0,.1), 0 4px 6px rgba(0,0,0,.05);
                transform: translateY(-2px);
            }

        .icon {
            width: 40px;
            height: 40px;
            margin-bottom: 0.75rem;
        }

        .muted {
            color: #6b7280;
        }

        .animate-fade-in {
            animation: fadeIn 0.3s ease-out;
        }

        @keyframes fadeIn {
            from {
                opacity: 0;
                transform: translateY(20px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

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

        /* OUTER WRAPPER CARD */
        .dashboard-wrapper {
            background: #dce3ea; /*#f8f9fa*/
            border-radius: 10px;
            padding: 25px;
            box-shadow: 0 4px 10px rgba(0,0,0,0.08);
        }

        /* INNER DASHBOARD CARD */
        .dashboard-card {
            background: #ffffff;
            border-radius: 8px;
            padding: 18px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.06);
            height: 100%;
        }

        /* Title smaller */
        .dashboard-card-title {
            font-size: 14px;
            font-weight: 600;
            margin-bottom: 12px;
            text-align: center;
            color: #374151;
        }

        /* Table font smaller */
        .dashboard-card table {
            font-size: 12px;
        }

        /* Spacing between cards */
        .dashboard-row > div {
            margin-bottom: 20px;
        }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <%--<script src="https://cdn.tailwindcss.com"></script>--%>
    <!-- Lucide icons -->
    <script src="https://cdn.jsdelivr.net/npm/lucide@0.469.0/dist/umd/lucide.min.js"></script>

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
        <h2 style="text-align: center; margin-top: 20px;">SO DashBoard</h2>

        <div class="headtag">
            <asp:Label ID="lblUserName" runat="server" Style="color: black; float: right; margin-top: 0px; margin-bottom: -20px; margin-right: 20px"></asp:Label><br />
            <asp:Label ID="lblGeo" runat="server" Style="color: black; float: right; margin-top: 0px; margin-bottom: -20px; margin-right: 20px"></asp:Label>
            <%--<asp:Label ID="lblDistCount" Text="DistCount : 30" runat="server" Style="color: black; float: right; margin-top: 0px; margin-bottom: -20px; margin-right: 20px"></asp:Label>--%>
        </div>

        <br />
        <br />

        <div class="container">
            <div class="row">
                <div class="col-6 col-md-3 mb-2 mb-md-0">
                    <div class="floating-label">
                        <asp:DropDownList ID="FYDrp" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="FYDrp_SelectedIndexChanged">
                            <%--<asp:ListItem Text="Select FY" Value=""></asp:ListItem>
                            <asp:ListItem Text="24-25" Value="2425"></asp:ListItem>
                            <asp:ListItem Text="25-26" Value="2526"></asp:ListItem>--%>
                        </asp:DropDownList>
                        <label for="FYDrp">FY</label>
                        <%--<asp:Label runat="server" Text="FY" AssociatedControlID="FYDrp" />--%>

                        <input type="text" id="FYSearch" runat="server" class="form-control" style="display: none" placeholder="Enter FY" />
                    </div>
                </div>

                <div class="col-6 col-md-3 mb-2 mb-md-0">
                    <div class="floating-label">
                        <asp:DropDownList ID="TypeDrp" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="TypeDrp_SelectedIndexChanged">
                            <asp:ListItem Text="Select Type" Value=""></asp:ListItem>
                            <asp:ListItem Text="Primary" Value="Primary"></asp:ListItem>
                            <asp:ListItem Text="Secondary" Value="Secondary"></asp:ListItem>
                            <asp:ListItem Text="Distributors" Value="Distributors"></asp:ListItem>
                        </asp:DropDownList>
                        <label for="TypeDrp">Type</label>
                        <%--<asp:Label runat="server" Text="FY" AssociatedControlID="FYDrp" />--%>
                    </div>
                </div>

                <div class="col-6 col-md-3 mb-2 mb-md-0">
                    <asp:Button ID="DistCountBtn" runat="server" CssClass="btn btn-outline-primary form-control" OnClick="DistCountBtn_Click" />
                </div>

                <div class="col-6 col-md-3 mb-2 mb-md-0">
                    <asp:Button ID="ExportBtn" runat="server" Text="Export" CssClass="btn btn-success form-control" OnClick="ExportBtn_Click" />
                </div>
            </div>
        </div>

        <div id="PriSecDiv" runat="server" visible="false">
            <div class="container mt-4">

                <!-- OUTER CARD (Sales Value) -->
                <div class="dashboard-wrapper">

                    <div class="dashboard-card-title" style="text-align: left; font-size: 20px;">
                        Sales Value
                    </div>

                    <div class="row dashboard-row">

                        <!-- Last Year -->
                        <div class="col-md-6">
                            <div class="dashboard-card">

                                <div class="dashboard-card-title">
                                    Last Year
                                </div>

                                <div class="table-responsive">
                                    <asp:GridView
                                        ID="gvSalesLastYear"
                                        runat="server"
                                        CssClass="table table-sm table-bordered text-center mb-0"
                                        AutoGenerateColumns="true"
                                        GridLines="Both">
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>

                        <!-- Plan -->
                        <div class="col-md-6">
                            <div class="dashboard-card">
                                <div class="dashboard-card-title">Plan</div>

                                <div class="table-responsive">
                                    <asp:GridView
                                        ID="gvSalesPlan"
                                        runat="server"
                                        CssClass="table table-sm table-bordered text-center mb-0"
                                        AutoGenerateColumns="true"
                                        GridLines="Both">
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>

                        <!-- Achievement -->
                        <div class="col-md-6">
                            <div class="dashboard-card">
                                <div class="dashboard-card-title">Achievement</div>

                                <div class="table-responsive">
                                    <asp:GridView
                                        ID="gvSalesAchievement"
                                        runat="server"
                                        CssClass="table table-sm table-bordered text-center mb-0"
                                        AutoGenerateColumns="true"
                                        GridLines="Both">
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>

                        <!-- % Achievement -->
                        <div class="col-md-6">
                            <div class="dashboard-card">

                                <div class="dashboard-card-title">% Achievement</div>

                                <div class="table-responsive">
                                    <asp:GridView
                                        ID="gvSalesPerAchievement"
                                        runat="server"
                                        CssClass="table table-sm table-bordered text-center mb-0"
                                        AutoGenerateColumns="true"
                                        GridLines="Both">
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>

                    </div>

                    <div class="row dashboard-row">

                        <!-- GOLY -->
                        <div class="col-md-12">
                            <div class="dashboard-card">
                                <div class="dashboard-card-title">GOLY</div>

                                <div class="table-responsive">
                                    <asp:GridView
                                        ID="gvSalesGoly"
                                        runat="server"
                                        CssClass="table table-sm table-bordered text-center mb-0"
                                        AutoGenerateColumns="true"
                                        GridLines="Both">
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>


                    </div>
                </div>

            </div>
            <div class="container mt-4">

                <!-- OUTER CARD (Brand Volume) -->
                <div class="dashboard-wrapper">

                    <div class="dashboard-card-title" style="text-align: left; font-size: 20px;">
                        Brand Volume
                    </div>

                    <div class="row dashboard-row">

                        <!-- Last Year -->
                        <div class="col-md-6">
                            <div class="dashboard-card">

                                <div class="dashboard-card-title">
                                    Last Year
                                </div>

                                <div class="table-responsive">
                                    <asp:GridView
                                        ID="gvBrandLastYear"
                                        runat="server"
                                        CssClass="table table-sm table-bordered text-center mb-0"
                                        AutoGenerateColumns="true"
                                        GridLines="Both">
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>

                        <!-- Plan -->
                        <div class="col-md-6">
                            <div class="dashboard-card">
                                <div class="dashboard-card-title">Plan</div>

                                <div class="table-responsive">
                                    <asp:GridView
                                        ID="gvBrandPlan"
                                        runat="server"
                                        CssClass="table table-sm table-bordered text-center mb-0"
                                        AutoGenerateColumns="true"
                                        GridLines="Both">
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>

                        <!-- Achievement -->
                        <div class="col-md-6">
                            <div class="dashboard-card">
                                <div class="dashboard-card-title">Achievement</div>

                                <div class="table-responsive">
                                    <asp:GridView
                                        ID="gvBrandAchievement"
                                        runat="server"
                                        CssClass="table table-sm table-bordered text-center mb-0"
                                        AutoGenerateColumns="true"
                                        GridLines="Both">
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>

                        <!-- % Achievement -->
                        <div class="col-md-6">
                            <div class="dashboard-card">

                                <div class="dashboard-card-title">% Achievement</div>

                                <div class="table-responsive">
                                    <asp:GridView
                                        ID="gvBrandPerAchievement"
                                        runat="server"
                                        CssClass="table table-sm table-bordered text-center mb-0"
                                        AutoGenerateColumns="true"
                                        GridLines="Both">
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>

                    </div>

                    <div class="row dashboard-row">

                        <!-- GOLY -->
                        <div class="col-md-12">
                            <div class="dashboard-card">
                                <div class="dashboard-card-title">GOLY</div>

                                <div class="table-responsive">
                                    <asp:GridView
                                        ID="gvBrandGoly"
                                        runat="server"
                                        CssClass="table table-sm table-bordered text-center mb-0"
                                        AutoGenerateColumns="true"
                                        GridLines="Both">
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>


                    </div>
                </div>

            </div>
        </div>

        <div id="distDiv" runat="server" visible="false">
            <div class="container mt-4">

                <!-- OUTER CARD (Distributor) -->
                <div class="dashboard-wrapper">

                    <div class="dashboard-card-title" style="text-align: left; font-size: 20px;">
                        Distributors
                    </div>

                    <div class="row dashboard-row">

                        <!-- Dist -->
                        <div class="col-md-12">
                            <div class="dashboard-card">
                                <%--<div class="dashboard-card-title">Dist</div>--%>

                                <div class="table-responsive" style="max-height: 200px; overflow-y: auto;">
                                    <asp:GridView
                                        ID="gvDistributors"
                                        runat="server"
                                        CssClass="table table-sm table-bordered text-center mb-0"
                                        AutoGenerateColumns="true"
                                        GridLines="Both">
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>

                    </div>

                </div>
            </div>
        </div>

        <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
        <asp:HiddenField ID="hdnBusinessType" runat="server" />
        <asp:HiddenField ID="hdnRole" runat="server" />
        <asp:HiddenField ID="hfSelectedRowData" runat="server" />
    </div>

    <script>
        // --- FYDrp autocomplete ---
        (function () {
            var ddl = $('#<%= FYDrp.ClientID %>');
            var searchBox = $('#<%= FYSearch.ClientID %>');

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
                    __doPostBack('<%= FYDrp.UniqueID %>', '');
                    return false;
                }
            });
        })();
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
