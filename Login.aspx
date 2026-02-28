<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SO_Appraisal.Login" Async="true" %>

<!DOCTYPE html>
<html>
<head>
    <title>Login</title>

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

        .card {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            padding: 2rem;
            background: #fff;
            border-radius: 0.75rem;
            box-shadow: 0 4px 10px rgba(0,0,0,.08);
            transition: background-color .2s ease, box-shadow .2s ease, transform .1s ease;
            text-align: center;
            font-weight: 600;
            color: #374151;
            width: 100%;
            max-width: 400px;
        }

            .card:hover {
                box-shadow: 0 8px 20px rgba(0,0,0,.12);
                transform: translateY(-2px);
            }

        .login-card input {
            font-weight: 500;
        }

        @media (max-width: 576px) {
            .card {
                padding: 1.5rem;
                max-width: 95%;
            }
        }
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
</head>
<body class="bg-light">
    <form runat="server">
        <div class="container vh-100 d-flex align-items-center justify-content-center">

            <div class="card login-card">

                <%--<h3 class="mb-4 text-primary">
                    <i class="fa fa-user-circle"></i>Login
                </h3>--%>

                <div class="form-group w-100 text-left">
                    <label>Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                </div>

                <div class="form-group w-100 text-left">
                    <label>Password</label>
                    <asp:TextBox ID="txtPassword" runat="server"
                        TextMode="Password" CssClass="form-control" />
                </div>

                <asp:Button ID="btnLogin"
                    runat="server"
                    Text="Login"
                    CssClass="btn btn-primary btn-block mt-3"
                    OnClick="btnLogin_Click" />

                <asp:Label ID="lblMessage" runat="server"
                    ForeColor="Red" CssClass="mt-2" />

            </div>

            <div id="toastContainer" aria-live="polite" aria-atomic="true"></div>

            <asp:HiddenField ID="hdnBusinessType" runat="server" />
            <asp:HiddenField ID="hdnRole" runat="server" />
            <asp:HiddenField ID="hfSelectedRowData" runat="server" />

        </div>
    </form>
</body>
</html>

