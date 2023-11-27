using System.Diagnostics;
using Common;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcApp.Models;

namespace MvcApp.Controllers;

[Authorize]
public class LabController : Controller
{
    private readonly ILogger<LabController> _logger;

    public LabController(ILogger<LabController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> FirstAsync([FromQuery] InputOutputModel inputOutput)
    {
        if (!string.IsNullOrWhiteSpace(inputOutput.InputValue))
        {
            try
            {
                var labFirst = new Lab1.Lab();

                var result = await labFirst.Execute(new(inputOutput.InputValue));

                inputOutput.OutputValue = result.Result;
            }
            catch (Exception exception)
            {
                inputOutput.OutputValue = exception.Message;
            }
        }

        return View(inputOutput);
    }

    public async Task<IActionResult> SecondAsync([FromQuery] InputOutputModel inputOutput)
    {
        if (!string.IsNullOrWhiteSpace(inputOutput.InputValue))
        {
            try
            {
                var labFirst = new Lab2.Lab();

                var result = await labFirst.Execute(new(inputOutput.InputValue));

                inputOutput.OutputValue = result.Result;
            }
            catch (Exception exception)
            {
                inputOutput.OutputValue = exception.Message;
            }
        }

        return View(inputOutput);
    }

    public async Task<IActionResult> ThirdAsync([FromQuery] InputOutputModel inputOutput)
    {
        if (!string.IsNullOrWhiteSpace(inputOutput.InputValue))
        {
            try
            {
                var labFirst = new Lab3.Lab();

                var result = await labFirst.Execute(new(inputOutput.InputValue));

                inputOutput.OutputValue = result.Result;
            }
            catch (Exception exception)
            {
                inputOutput.OutputValue = exception.Message;
            }
        }

        return View(inputOutput);
    }
}
