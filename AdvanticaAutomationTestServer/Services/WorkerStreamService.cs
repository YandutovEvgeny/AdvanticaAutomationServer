using AdvanticaAutomationTestServer.Infractructure;
using AdvanticaAutomationTestServer.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestServer.Services
{
    public class WorkerStreamService : WorkerIntegration.WorkerIntegrationBase
    {
        WorkerContext workerContext;
        public WorkerStreamService(WorkerContext workerContext)
        {
            this.workerContext = workerContext;
        }

        public override async Task GetWorkerStream(EmptyMessage request, IServerStreamWriter<WorkerAction> responseStream, ServerCallContext context)
        {
            var workerMessageList = workerContext.Workers.Select(item => new WorkerMessage
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                MiddleName = item.MiddleName,
                Sex = (Sex)item.Sex,
                Birthday = Timestamp.FromDateTimeOffset(item.Birthday),
                HaveChildren = item.HaveChildren
            });

            foreach (var worker in workerMessageList)
            {
                await responseStream.WriteAsync(new WorkerAction { Worker = worker });
            }
        }

        public override async Task<WorkerMessage> CreateWorker(WorkerAction request, ServerCallContext context)
        {
            var worker = new Worker
            {
                FirstName = request.Worker.FirstName,
                LastName = request.Worker.LastName,
                MiddleName = request.Worker.MiddleName,
                HaveChildren = request.Worker.HaveChildren,
                Birthday = request.Worker.Birthday.ToDateTime(),
                Sex = (Enums.Sex)request.Worker.Sex
            };

            await workerContext.Workers.AddAsync(worker);
            await workerContext.SaveChangesAsync();

            var response = new WorkerMessage
            {
                Id = worker.Id,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                MiddleName = worker.MiddleName,
                HaveChildren = worker.HaveChildren,
                Birthday = Timestamp.FromDateTimeOffset(worker.Birthday),
                Sex = (Sex)worker.Sex
            };

            return await Task.FromResult(response);
        }

        public override async Task<WorkerMessage> EditWorker(WorkerAction request, ServerCallContext context)
        {
            var worker = await workerContext.Workers.FindAsync(request.Worker.Id);

            if(worker == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Worker with id:{request.Worker.Id} not found"));
            }

            worker.FirstName = request.Worker.FirstName;
            worker.LastName = request.Worker.LastName;
            worker.MiddleName = request.Worker.MiddleName;
            worker.Birthday = request.Worker.Birthday.ToDateTime();
            worker.HaveChildren = request.Worker.HaveChildren;
            worker.Sex = (Enums.Sex)request.Worker.Sex;

            await workerContext.SaveChangesAsync();

            var response = new WorkerMessage
            {
                Id = worker.Id,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                MiddleName = worker.MiddleName,
                HaveChildren = worker.HaveChildren,
                Birthday = Timestamp.FromDateTimeOffset(worker.Birthday),
                Sex = (Sex)worker.Sex
            };

            return await Task.FromResult(response);
        }

        public override async Task<WorkerMessage> DeleteWorker(WorkerAction request, ServerCallContext context)
        {
            var worker = await workerContext.Workers.FindAsync(request.Worker.Id);
            
            if(worker == null) 
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Worker with id:{request.Worker.Id} not found"));
            }

            workerContext.Workers.Remove(worker);
            await workerContext.SaveChangesAsync();

            var response = new WorkerMessage
            {
                Id = worker.Id,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                MiddleName = worker.MiddleName,
                HaveChildren = worker.HaveChildren,
                Birthday = Timestamp.FromDateTimeOffset(worker.Birthday),
                Sex = (Sex)worker.Sex
            };

            return await Task.FromResult(response);
        }

        public override async Task<WorkerMessage> FindWorkerById(WorkerId request, ServerCallContext context)
        {
            var worker = await workerContext.Workers.FindAsync(request.Id);

            if (worker == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Worker with id:{request.Id} not found"));
            }

            return await Task.FromResult(new WorkerMessage
            {
                Id = worker.Id,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                MiddleName = worker.MiddleName,
                HaveChildren = worker.HaveChildren,
                Birthday = Timestamp.FromDateTimeOffset(worker.Birthday),
                Sex = (Sex)worker.Sex
            });
        }
    }
}
