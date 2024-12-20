using homework_3.Models;
using Microsoft.AspNetCore.Mvc;

namespace homework_3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : Controller
{
    private static List<HomeTask> tasks = new List<HomeTask>
    {
        new HomeTask {Id = 1, Title = "Sample Task", Description = "This is a sample task.", IsCompleted = false},
    };
    
    [HttpGet]
    public IActionResult GetAll() => Ok(tasks);

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var task = tasks.FirstOrDefault(t => t.Id == id);
        return task == null ? NotFound() : Ok(task);
    }

    [HttpPost]
    public IActionResult Create(HomeTask newTask)
    {
        newTask.Id = tasks.Count + 1;
        tasks.Add(newTask);
        return CreatedAtAction(nameof(GetById), new {id = newTask.Id}, newTask);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, HomeTask updateTask)
    {
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            return NotFound();
        }
        
        task.Title = updateTask.Title;
        task.Description = updateTask.Description;
        task.IsCompleted = updateTask.IsCompleted;
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            return NotFound();
        }
        
        tasks.Remove(task);
        return NoContent();
    }
}